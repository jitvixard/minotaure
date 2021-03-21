using System;
using src.actors.controllers;
using UnityEngine;

namespace src.ai
{
    public abstract class AbstractStateMachine : MonoBehaviour
    {
        /*===============================
         *  Fields
         ==============================*/
        [NonSerialized] public AbstractActorController controller;
        
        State currentState = State.Idle; //Idle is entry state
        
        
        /*===============================
         *  Properties
         ==============================*/
        public State CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                UpdateState();
            }
        }

        
        /*===============================
         *  Unity Lifecycle
         ==============================*/
        protected abstract void Awake();


        /*===============================
         *  States
         ==============================*/
        protected void Idle()
        {
            controller.Idle();
        }
        
        protected void Seek(Transform transform)
        {
            print("seek");
            currentState = State.Seek;
            controller.Seek(transform);
        }

        protected void Attack()
        {
            print("attack");
            currentState = State.Attack;
            controller.Attack();
        }

        protected void Regroup()
        {
            print("regroup");
            currentState = State.Regroup;
        }
        

        /*===============================
         *  Control Functions
         ==============================*/
        public void Resume()
        {
            enabled = true;
        }
        
        public void Stop()
        {
            currentState = State.Stopped;
            enabled = false;
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
            return currentState == State.Idle
                && !(controller.Target is null);
        }
        
        protected bool ShouldIdle()
        {
            return currentState == State.Idle;
        }
        
        protected bool ShouldRegroup()
        {
            //return !controller.InHeatZone;
            return false;
        }
    }
}
