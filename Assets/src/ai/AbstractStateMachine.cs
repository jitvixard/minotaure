using System;
using System.Collections;
using System.Diagnostics;
using src.actors.controllers;
using UnityEngine;
using Environment = src.util.Environment;
using Random = UnityEngine.Random;

namespace src.ai
{
    public abstract class AbstractStateMachine : MonoBehaviour 
    {
        public ActorController Controller { get; set; }

        public State State { get; set; }

        protected Coroutine idleRoutine;
        protected Coroutine attackRoutine;
        protected Coroutine regroupRoutine;

        protected Vector3 idleOrigin;

        public void UpdateState()
        {
            if (idleRoutine == null) idleRoutine = StartCoroutine(IdleRoutine());
        }
        
        
        /*
         *  Directives
         */
        protected virtual void Idle()
        {
            if (idleRoutine is null)
            {
                idleOrigin = transform.position;
            }
            
            idleRoutine = StartCoroutine(IdleRoutine());
        }

        protected virtual void Attack()
        {
            
        }

        protected virtual void Regroup()
        {
            
        }
        
        /*
         *  Routines
         */
        protected virtual IEnumerator IdleRoutine()
        {
            var watch = new Stopwatch();
            watch.Start();
            var waitTime = Random.Range(Environment.IDLE_WAIT_LOWER, Environment.IDLE_WAIT_LOWER) * 1000;
            waitTime += watch.ElapsedMilliseconds;

            while (watch.ElapsedMilliseconds < waitTime) yield return null; //waiting
            
            Controller.Move(GetLocationAroundUnit(Environment.IDLE_RANGE)); //move to random point

            while (Controller.Actor.Moving) yield return null; //waiting

            if (State == State.Idle) Idle(); //refresh idle state
        }
        
        protected virtual IEnumerator AttackRoutine()
        {
            //moveTo
            //combat
            //stop
            yield break;
        }
        
        protected virtual IEnumerator RegroupRoutine()
        {
            //are others regrouping?
            //create or find position
            //wait
            yield break;
        }
        
        protected virtual void Stop()
        {
            if (!(idleRoutine is null))
            {
                StopCoroutine(idleRoutine);
                idleRoutine = null;
            }
        }
        
        /*
         *  Checks
         */
        void Update()
        {
            UpdateState();
        }

        /*
         *  Helper Methods
         */
        protected Vector3 GetLocationAroundUnit(int radius)
        {
            var randomPoint = (Random.insideUnitCircle * radius);
            return new Vector3(
                idleOrigin.x + randomPoint.x,
                idleOrigin.y,
                idleOrigin.z + randomPoint.y
            );
        }
    }
}
