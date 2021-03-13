using src.actors.model;

namespace src.ai
{
    public class StateMachineFactory
    {
        public static AbstractStateMachine Get(AbstractActor actor)
        {
            switch (actor)
            {
                case PawnActor p:
                    var stateMachine = actor.GameObject.AddComponent<PawnStateMachine>();
                    stateMachine.Controller = actor.Controller;
                    return stateMachine;
                default:
                    return null;
            }
        }
    }
}