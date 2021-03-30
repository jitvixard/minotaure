using System.Collections;
using src.actors.handlers.sprite;
using src.buildings.controllers;
using src.interfaces;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        /*===============================
         *  Fields
         ==============================*/
        IDestroyable destroyable;

        PawnActorController player;

        bool attackingPlayer;

        float extraReach;



        /*===============================
         *  LifeCycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            sprite = new SpriteHandler(this);

            builderService.BuildingDestroyed += BuildingDestroyed;
            playerService.Player             += PlayerChanged;
        }

        public override void Die()
        {
            Instantiate(prototypeExplosion, transform.position, new Quaternion());

            swarmService.Remove(this);
            Destroy(gameObject);
        }

        protected override void OnDestroy()
        {
            builderService.BuildingDestroyed -= BuildingDestroyed;
            playerService.Player             -= PlayerChanged;
        }

        public void Ready(GameObject target, bool attackingPlayer)
        {
            this.target = target;
            
            if (target.TryGetComponent<PawnActorController>(out var pac))
                destroyable = pac;
            else if (target.TryGetComponent<BuildingController>(out var bc))
                destroyable = bc;
            
            this.attackingPlayer = attackingPlayer;

            Attack();
        }

        
        
        /*===============================
         *  Subscriptions
         ==============================*/
        void BuildingDestroyed(GameObject destroyed)
        {
            if (attackingPlayer) LoadPlayerReference();
            else if (destroyed == target) LoadNewInfrastructureTarget();
        }
        
        void PlayerChanged(PawnActorController player)
        { 
            if (player is null)
            { 
                if (currentRoutine != null) StopCoroutine(currentRoutine); 
                return;
            }
            
            this.player = player;
            LoadPlayerReference(player); 
        }


        /*===============================
         *  IDestroyable
         ==============================*/
        public override bool Damage(AbstractActorController actorController)
        {
            Die();
            return true;
        }

        public override float ExtraOffset()
        {
            return 0f;
        }
        
        
        
        /*===============================
         *  Handling
         ==============================*/
        void Attack()
        {
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(AttackRoutine());
        }
        
        
        
        /*===============================
         *  Routines
         ==============================*/
        void LoadPlayerReference()
        {
            target = player.gameObject;
            LoadPlayerReference(player);
        }
        
        void LoadPlayerReference(IDestroyable destroyable)
        {
            if (attackingPlayer)
            {
                this.destroyable = destroyable;
                target           = player.gameObject;
                Attack();
            }
        }

        void LoadNewInfrastructureTarget()
        {
            var destructibles = builderService.Destructibles;
            if (destructibles.Length < 1)
            {
                attackingPlayer = true;
                LoadPlayerReference();
                return;
            }
            
            var closest = destructibles[0];
            var position = transform.position;
            foreach (var dx in destructibles)
            {
                if (Vector3.Distance(closest.GetTransform().position, position) >
                    Vector3.Distance(dx.GetTransform().position, position))
                {
                    closest = dx;
                }
            }

            destroyable = closest;
            target      = closest.GetTransform().gameObject;
            
            Attack();
        }
        
        /*===============================
         *  Routines
         ==============================*/
        IEnumerator AttackRoutine()
        {
            var lookRoutine = StartCoroutine(LookRoutine());
            
            var targetTransform = target.transform;

            while (destroyable.Health() > 0)
            {
                //moving section of the routine *******************************
                agent.SetDestination(targetTransform.position);
                while (agent.remainingDistance == 0) yield return null;

                //keep moving till in range ***********************************
                while (!InRangeToAttack(transform.position, targetTransform.position))
                {
                    agent.SetDestination(targetTransform.position);
                    yield return null;
                }

                //attack then check if we need to move ************************
                do
                {
                    sprite.Slash(destroyable);
                    if (destroyable.Health() <= 0)
                    {
                        StopCoroutine(lookRoutine);
                        yield break;
                    }
                    yield return null;
                } while (destroyable != null
                && InRangeToAttack(transform.position, targetTransform.position));

                yield return null;
            }

            StopCoroutine(lookRoutine);
            agent.SetDestination(transform.position);
        }

        IEnumerator LookRoutine()
        {
            transform.LookAt(target.transform);
            yield return null;
        }
        
        
        
        /*===============================
         *  Utility
         ==============================*/
        bool InRangeToAttack(Vector3 currentPosition, Vector3 targetPosition)
        {
            var range = Environment.SWARM_ATTACKING_RANGE;
            if (attackingPlayer)
                return Vector3.Distance(currentPosition, targetPosition) < range;

            range += destroyable.ExtraOffset();
            return Vector3.Distance(currentPosition, targetPosition) < range;
        }
    }
}