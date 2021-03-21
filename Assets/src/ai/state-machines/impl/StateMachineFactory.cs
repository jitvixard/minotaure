using src.actors.model;
using src.ai.impl;

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
                    stateMachine = actor.gameObject.AddComponent<PawnStateMachine>();
                    break;
                case SwarmActor s:
                    stateMachine = actor.gameObject.AddComponent<SwarmStateMachine>();
                    break;
                default:
                    return null;
            }

            stateMachine.controller = actor.controller;
            return stateMachine;
        }
    }
}