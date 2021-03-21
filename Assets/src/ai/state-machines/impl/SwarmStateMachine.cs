using src.actors.controllers.impl;

namespace src.ai.impl
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
            if (ShouldSeek()) Seek(controller.Player.transform);
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
            CurrentState = State.Locate;
            controller.Locate();
        }





    }
}