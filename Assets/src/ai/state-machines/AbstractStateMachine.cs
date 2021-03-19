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
        [NonSerialized] public AbstractActorController controller;

        public State currentState;

        protected Coroutine idleRoutine;
        protected Coroutine attackRoutine;
        protected Coroutine regroupRoutine;

        protected Vector3 idleOrigin;


        /*===============================
         *  States
         ==============================*/
        protected virtual void Idle()
        {
            if (currentState != State.Idle)
            {
                idleOrigin = transform.position;
                currentState = State.Idle;
            }
            if (idleRoutine is null) idleRoutine = StartCoroutine(IdleRoutine());
        }

        protected virtual void Attack()
        {
            currentState = State.Attack;
        }

        protected virtual void Regroup()
        {
            currentState = State.Regroup;
        }
        
        /*===============================
         *  Routines
         ==============================*/
        protected virtual IEnumerator IdleRoutine()
        {
            var watch = new Stopwatch();
            watch.Start();
            var waitTime = Random.Range(Environment.IDLE_WAIT_LOWER, Environment.IDLE_WAIT_LOWER) * 1000;
            waitTime += watch.ElapsedMilliseconds;

            while (watch.ElapsedMilliseconds < waitTime) yield return null; //waiting
            
            controller.Move(GetLocationAroundUnit(Environment.IDLE_RANGE)); //move to random point

            while (controller.actor.Moving) yield return null; //waiting

            idleRoutine = null; //remove this

            if (currentState == State.Idle) Idle(); //refresh idle state
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
        
        /*===============================
         *  Control Functions
         ==============================*/
        public virtual void Resume()
        {
            enabled = true;
        }
        
        public virtual void Stop()
        {
            currentState = State.Stopped;
            enabled = false;
            
            if (!(idleRoutine is null))
            {
                StopCoroutine(idleRoutine);
                idleRoutine = null;
            }
            if (!(attackRoutine is null))
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
            if (!(regroupRoutine is null))
            {
                StopCoroutine(regroupRoutine);
                regroupRoutine = null;
            }
        }
        
        /*===============================
         *  Checks
         ==============================*/
        protected void Update()
        {
            UpdateState();
        }

        protected virtual void UpdateState()
        {
            if (ShouldIdle()) Idle();
            if (ShouldAttack()) Attack();
            if (ShouldRegroup()) Regroup();
        }
        protected virtual bool ShouldAttack()
        {
            return false;
        }
        
        protected virtual bool ShouldIdle()
        {
            return false;
        }
        
        protected virtual bool ShouldRegroup()
        {
            return false;
        }

        /*===============================
         *  Helper
         ==============================*/
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
