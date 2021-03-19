namespace src.ai.impl
{
    public class SwarmStateMachine : AbstractStateMachine
    {
        //TODO override AbstractActorController
        
        /*===============================
         *  Checks
         ==============================*/
        protected override void UpdateState()
        {
            if (ShouldLocate()) Locate();
        }

        protected new virtual void Update()
        {
            
        }

        protected override bool ShouldAttack()
        {
            
        }
        
        bool ShouldLocate()
        {
            
        }
        
        //default behaviour
        bool ShouldSeek()
        {
            
        }
        
        
        /*===============================
         *  States
         ==============================*/
        protected override void Attack()
        {
            
        }

        void Locate()
        {
            
        }

        void Seek()
        {
            
        }




    }
}