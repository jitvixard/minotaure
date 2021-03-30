using src.actors.controllers;
using src.actors.controllers.impl;
using src.util;
using UnityEngine;

namespace src.actors.model
{
    public abstract class AbstractActor
    {
        public AbstractActorController controller;
        public GameObject              gameObject;
        public int                     health;

        public bool  moving;

        protected AbstractActor(AbstractActorController controller)
        {
            gameObject = controller.gameObject;
            this.controller = controller;
            moving = false;
            health = 100;
        }
    }

    public static class ActorFactory
    {
        public static AbstractActor Create(AbstractActorController controller)
        {
            switch (controller.tag)
            {
                case Environment.TAG_PAWN:
                    return new PawnActor(controller);

                case Environment.TAG_SWARM:
                    return new SwarmActor(controller);
                
                case Environment.TAG_BUILDER:
                    return new BuilderActor(controller);

                default:
                    Debug.LogWarning("Defaulting Actor [" + controller.name + "]");
                    return new PawnActor(controller);
            }
        }

        public static AbstractActor Create(SwarmActorController controller)
        {
            return new SwarmActor(controller);
        }
    }
}