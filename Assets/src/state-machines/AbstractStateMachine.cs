using src.actors.controllers;
using src.model;
using src.services.impl;
using UnityEngine;
using Environment = src.util.Environment;

namespace src
{
    public abstract class AbstractStateMachine : MonoBehaviour
    {
        /*===============================
         *  Fields
         ==============================*/
        protected AbstractActorController controller;
        protected PlayerService           playerService;

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
                if (currentState == value) return;
                currentState = value;
                if (firstFrame) return;
                CheckState();
            }
        }


        /*===============================
         *  Unity Lifecycle
         ==============================*/
        protected virtual void Awake()
        {
            controller    = GetComponent<AbstractActorController>();
            playerService = Environment.PlayerService;
        }

        protected void Update()
        {
            if (firstFrame)
            {
                CheckState();
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
        public virtual void CheckState()
        {
            if (ShouldIdle()) Idle();
        }

        bool ShouldIdle()
        {
            return currentState == State.Idle;
        }
    }
}