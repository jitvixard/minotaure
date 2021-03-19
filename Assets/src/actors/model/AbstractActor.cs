using System.Collections.Generic;
using src.actors.controllers;
using src.actors.model;
using src.util;
using UnityEngine;

namespace src.actors.model
{
    public abstract class AbstractActor
    {
        public GameObject GameObject { get; set; }
        public AbstractActorController Controller { get; set; }

        public bool Moving { get; set; }

        public int Health { get; set; }
        public float Speed { get; set; }

        protected AbstractActor(AbstractActorController controller)
        {
            GameObject = controller.gameObject;
            Controller = controller;
            Moving = false;
            Health = 100;
            Speed = controller.Agent.speed;
        }
    }
}
public static class ActorFactory
{
    //TODO get all actors from JSON
    public static AbstractActor Create(AbstractActorController controller)
    {
        Debug.Log("Defaulting Actor [" + controller.name + "]");
        switch (controller.tag)
        {
            case Environment.TAG_PAWN:
                return new PawnActor(controller);
            default:
                Debug.LogWarning("Defaulting Actor [" + controller.name + "]");
                return new PawnActor(controller);
        }
            
    }
}