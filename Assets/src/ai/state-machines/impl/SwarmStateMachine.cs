using src.actors.controllers.impl;
using UnityEngine.PlayerLoop;

namespace src.ai
{
    public class SwarmStateMachine : AbstractStateMachine
    {
        new SwarmActorController controller;


        /*===============================
         *  Unity Lifecycle
         ==============================*/
        protected override void Awake()
        {
            controller = GetComponent<SwarmActorController>();
        }

        /*===============================
         *  Checks
         ==============================*/
        protected override void UpdateState()
        {
            //seek -> locate -> attack -> seek (repeat)
            if (ShouldSeek()) Seek();
            if (ShouldLocate()) Locate();
            if (ShouldAttack()) Attack();
        }
        
        //default behaviour
        bool ShouldSeek()
        {
            //TODO Are they IDLE or not in another state
            return currentState == State.Idle;
        }
        
        bool ShouldLocate()
        {
            return controller.InHeatZone 
                && currentState != State.Attack
                && currentState != State.Locate;
        }

        protected override bool ShouldAttack()
        {
            //TODO is the actor in range of the target POI and are they in range fo the player
            return false;
        }


        /*===============================
         *  States
         ==============================*/
        void Seek()
        {
            currentState = State.Seek;
            controller.Seek();
        }
        
        void Locate()
        {
            currentState = State.Locate;
            controller.Locate();
        }

        protected override void Attack()
        {
            currentState = State.Attack;
            controller.Attack();
        }





    }
}