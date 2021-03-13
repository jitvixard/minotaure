using System;
using src.controllers;
using UnityEngine;

namespace src.ai
{
    public abstract class AbstractStateMachine : MonoBehaviour 
    {
        public ActorController Controller { get; set; } 

        public abstract void UpdateState();
        
        
        /*
         *  Directives
         */
        public abstract void Idle();
        public abstract void Attack();
        public abstract void Regroup();
        
        /*
         *  Checks
         */

        void Update()
        {
            UpdateState();
        }
    }
}
