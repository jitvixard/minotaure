using System.Collections;
using src.actors.handlers.sprite;
using src.interfaces;
using src.model;
using src.util;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        /*===============================
         *  LifeCycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            
            sprite =  new SpriteHandler(this);
        }

        public override void Die()
        {
            print(name + " dying");
            swarmService.Remove(this);
            Destroy(gameObject);
        }


        /*===============================
         *  Actions
         ==============================*/
        public void Attack()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(AttackRoutine());
        }
        
        public void Locate()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(LocateRoutine());
        }

        /*===============================
         *  Routines
         ==============================*/

        IEnumerator AttackRoutine()
        {
            var targetController = target.GetComponent<PawnActorController>();
            //TODO set building
            var destroyable = (IDestroyable) targetController;

            agent.SetDestination(target.transform.position);
            
            while (destroyable.Health() > 0)
            {
                agent.ResetPath();
                agent.SetDestination(target.transform.position);
                var tries = 0;
                while (agent.remainingDistance == 0)
                {
                    if (tries++ == 9)
                    {
                        tries = 0;
                        agent.SetDestination(target.transform.position);
                    } 
                    yield return null;
                }
                
                while (agent.remainingDistance > Environment.SWARM_ATTACKING_RANGE)
                {
                    var pos = target.transform.position;
                    agent.SetDestination(pos);
                    transform.LookAt(pos);
                    yield return null;
                }

                if (Vector3.Distance(
                    targetController.transform.position, 
                    transform.position) 
                    < Environment.SWARM_ATTACKING_RANGE) 
                    sprite.Slash(targetController);
                agent.isStopped = true;
                
                var t = 0f;
                while (t < Environment.SWARM_ATTACK_DELAY)
                {
                    t += Time.deltaTime;
                    yield return null;
                }

                agent.isStopped = true;
                agent.SetDestination(target.transform.position);
                
                agent.autoBraking = true;
            }

            agent.SetDestination(transform.position);
            currentRoutine            = null;
            stateMachine.CurrentState = State.Idle;
        }
        
        IEnumerator LocateRoutine()
        {
            var attempts = 0;
            target = null;
            
            while (target is null
                   && attempts++ < Environment.SWARM_MAX_LOCATE_ATTEMPTS)
            {
                yield return null;
                target = swarmService.GetTarget(this); //TODO Check this
            }

            if (target is null)
                target = playerService.PlayerPawn.gameObject;

            stateMachine.CurrentState = State.Attack;
        }
    }
}