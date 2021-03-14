using src.controllers;
using UnityEngine;

namespace src.actors.model
{
    public abstract class AbstractActor
    {
        public GameObject GameObject { get; set; }
        public ActorController Controller { get; set; }

        public bool Moving { get; set; } 

        public int Health { get; set; }
        public float Speed { get; set; }

        protected AbstractActor(ActorController controller)
        {
            GameObject = controller.gameObject;
            Controller = controller;
            Moving = false;
            Health = 100;
            Speed = controller.Agent.speed;
        }
        
    }
}