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

        public State CurrentState { get; set; }

        protected Coroutine idleRoutine;
        protected Coroutine attackRoutine;
        protected Coroutine regroupRoutine;

        protected Vector3 idleOrigin;

        public void UpdateState()
        {
            if (idleRoutine == null) idleRoutine = StartCoroutine(IdleRoutine());
        }
        
        
        /*===============================
         *  State Directives
         ==============================*/
        protected virtual void Idle()
        {
            if (CurrentState != State.Idle) idleOrigin = transform.position;
            CurrentState = State.Idle;
            idleRoutine = StartCoroutine(IdleRoutine());
        }

        protected virtual void Attack()
        {
            CurrentState = State.Attack;
        }

        protected virtual void Regroup()
        {
            CurrentState = State.Regroup;
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
            
            Controller.Move(GetLocationAroundUnit(Environment.IDLE_RANGE)); //move to random point

            while (Controller.Actor.Moving) yield return null; //waiting

            if (CurrentState == State.Idle) Idle(); //refresh idle state
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
            CurrentState = State.Stopped;
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
        void Update()
        {
            UpdateState();
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
