using src.actors.controllers;
using src.util;
using UnityEngine;

namespace src.actors.model
{
    public abstract class AbstractActor
    {
        public int                     attackRate;
        public AbstractActorController controller;
        public int                     damage;
        public GameObject              gameObject;
        public int                     health;

        public bool  moving;
        public float speed;

        protected AbstractActor(AbstractActorController controller)
        {
            gameObject = controller.gameObject;
            this.controller = controller;
            moving = false;
            health = 100;
            speed = controller.agent.speed;
            damage = 10;
        }
    }

    public static class ActorFactory
    {
        //TODO get all actors from JSON
        public static AbstractActor Create(AbstractActorController controller)
        {
            switch (controller.tag)
            {
                case Environment.TAG_PAWN:
                    return new PawnActor(controller) {attackRate = 1000};

                case Environment.TAG_SWARM:
                    return new SwarmActor(controller) {attackRate = Environment.SwarmService.AttackRate};

                default:
                    Debug.LogWarning("Defaulting Actor [" + controller.name + "]");
                    return new PawnActor(controller);
            }
        }
    }
}