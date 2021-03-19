using src.actors.model;

namespace src.ai
{
    public static class StateMachineFactory
    {
        public static AbstractStateMachine Get(AbstractActor actor)
        {
            AbstractStateMachine stateMachine;
            
            switch (actor)
            {
                case PawnActor p:
                    stateMachine = actor.GameObject.AddComponent<PawnStateMachine>();
                    break;
                case SwarmActor s:
                    stateMachine = actor.GameObject.AddComponent<SwarmStateMachine>();
                    break;
                default:
                    return null;
            }

            stateMachine.controller = actor.Controller;
            return stateMachine;
        }
    }
}