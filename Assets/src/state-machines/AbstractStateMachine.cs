using System;
using src.actors.controllers;
using src.model;
using UnityEngine;

namespace src
{
    public abstract class AbstractStateMachine : MonoBehaviour
    {
        /*===============================
         *  Fields
         ==============================*/
        [NonSerialized] public AbstractActorController controller;
        
        State currentState; //Idle is entry state

        bool firstFrame = true;
        
        
        /*===============================
         *  Properties
         ==============================*/
        public State CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                if (firstFrame) return;
                UpdateState();
            }
        }

        
        /*===============================
         *  Unity Lifecycle
         ==============================*/
        protected virtual void Awake()
        {
            controller = GetComponent<AbstractActorController>();
        }

        protected void Update()
        {
            if (firstFrame)
            {
                UpdateState();
                firstFrame = false;
            }
        }


        /*===============================
         *  States
         ==============================*/
        protected void Idle()
        {
            controller.Idle();
        }
        
        protected void Seek(Transform transform)
        {
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
        protected virtual void UpdateState()
        {
            if (ShouldIdle()) Idle();
            if (ShouldAttack()) Attack();
            if (ShouldRegroup()) Regroup();
        }
        
        protected virtual bool ShouldAttack()
        {
            return currentState == State.Idle
                   && !(controller.Target is null)
                   && controller.InRange;
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
