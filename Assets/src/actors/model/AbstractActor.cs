using src.controllers;
using UnityEngine;

namespace src.actors.model
{
    public abstract class AbstractActor
    {
        public GameObject GameObject { get; set; }
        public ActorController Controller { get; set; }
        
        public int Health { get; set; }
        public int Speed { get; set; }
    }
}