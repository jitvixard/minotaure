using System.Collections;
using src.actors.handlers.sprite;
using src.actors.model;
using src.interfaces;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        /*===============================
         *  Fields
         ==============================*/
        IDestroyable destroyable;

        bool attackingPlayer;

        float extraReach;



        /*===============================
         *  LifeCycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            sprite = new SpriteHandler(this);
        }

        public override void Die()
        {
            Instantiate(prototypeExplosion, transform.position, new Quaternion());

            swarmService.Remove(this);
            Destroy(gameObject);
        }

        public void Ready(GameObject target, bool attackingPlayer)
        {
            this.target = target;
            if (target.TryGetComponent<PawnActorController>(out var pac))
                destroyable = pac;

            this.attackingPlayer = attackingPlayer;

            currentRoutine = StartCoroutine(AttackRoutine());
        }



        /*===============================
         *  IDestroyable
         ==============================*/
        public override void Damage(AbstractActorController actorController)
        {
            Die();
        }

        public override float ExtraOffset()
        {
            return 0f;
        }

        /*===============================
         *  Routines
         ==============================*/
        IEnumerator AttackRoutine()
        {
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
                    transform.LookAt(targetTransform);
                    yield return null;
                }

                //attack then check if we need to move ************************
                do
                {
                    sprite.Slash(destroyable);
                    yield return null;
                } while (InRangeToAttack(transform.position, targetTransform.position));

                yield return null;
            }

            agent.SetDestination(transform.position);
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