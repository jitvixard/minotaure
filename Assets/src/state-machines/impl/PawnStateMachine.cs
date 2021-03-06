using src.actors.controllers.impl;

namespace src.impl
{
    public class PawnStateMachine : AbstractStateMachine
    {
        protected override void Awake()
        {
            controller = GetComponent<PawnActorController>();
        }
    }
}