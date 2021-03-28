using src.actors.model;

namespace src.impl
{
    public static class StateMachineFactory
    {
        public static AbstractStateMachine Create(AbstractActor actor)
        {
            AbstractStateMachine stateMachine;

            switch (actor)
            {
                case PawnActor p:
                    stateMachine = actor.gameObject.AddComponent<PawnStateMachine>();
                    break;
                case SwarmActor s:
                    stateMachine = actor.gameObject.AddComponent<SwarmStateMachine>();
                    break;
                default:
                    return null;
            }

            return stateMachine;
        }
    }
}