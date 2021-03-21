using src.actors.controllers.impl;
using src.ai.swarm;
using src.util;

namespace src.ai.impl
{
    public class SwarmStateMachine : AbstractStateMachine
    {
        new SwarmActorController controller;
        readonly SwarmService swarmService = Environment.SwarmService;


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
            if (ShouldSeek()) Seek(swarmService.GetTarget(controller).transform); //defaults to player atm
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
                && CurrentState == State.Locate;
        }


        /*===============================
         *  States
         ==============================*/
        void Locate()
        {
            print("Locate");
            CurrentState = State.Locate;
            controller.Locate();
        }





    }
}