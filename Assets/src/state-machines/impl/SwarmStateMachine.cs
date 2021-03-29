using src.actors.controllers.impl;
using src.model;
using src.util;
using UnityEngine;

namespace src.impl
{
    public class SwarmStateMachine : AbstractStateMachine
    {
        SwarmActorController  sac;


        /*===============================
         *  Unity Lifecycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            sac = GetComponent<SwarmActorController>();
        }

        /*===============================
         *  Checks
         ==============================*/
        public override void CheckState()
        {
            //seek -> locate -> attack -> seek (repeat)
            if (ShouldSeek()) Seek(); //defaults to player atm
            if (ShouldLocate()) Locate();
            if (ShouldAttack()) Attack();
        }

        //default behaviour
        bool ShouldSeek()
        {
            return CurrentState == State.Idle;
        }

        bool ShouldLocate()
        {
            return controller.InHeatZone
                   && CurrentState == State.Seek;
        }

        bool ShouldAttack()
        {
            return !(controller.Target is null)
                   && CurrentState == State.Attack;
        }


        /*===============================
         *  States
         ==============================*/
        void Attack()
        {
            print("attack");
            CurrentState = State.Attack;
            sac.Attack();
        }
        
        void Seek()
        {
            CurrentState = State.Seek;
            controller.Seek(playerService.PlayerPawn.transform);
        }
        
        void Locate()
        {
            print("Locate");
            CurrentState = State.Locate;
            sac.Locate();
        }
    }
}