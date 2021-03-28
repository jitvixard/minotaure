using src.actors.controllers.impl;
using src.model;
using src.services.impl;
using src.util;
using UnityEngine;

namespace src.impl
{
    public class SwarmStateMachine : AbstractStateMachine
    {
        readonly SwarmService swarmService = Environment.SwarmService;
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
        protected override void UpdateState()
        {
            //seek -> locate -> attack -> seek (repeat)
            if (ShouldSeek()) Seek(swarmService.GetTarget(sac).transform); //defaults to player atm
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
                   && CurrentState != State.Attack
                   && CurrentState != State.Locate;
        }

        protected override bool ShouldAttack()
        {
            return !(controller.Target is null)
                   && CurrentState == State.Locate
                   && Vector3.Distance(
                       transform.position,
                       controller.Target.transform.position)
                   < Environment.COMBAT_ATTACK_RANGE;
        }


        /*===============================
         *  States
         ==============================*/
        void Locate()
        {
            print("Locate");
            CurrentState = State.Locate;
            sac.Locate();
        }
    }
}