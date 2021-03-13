using src.actors.model;
using src.controllers;
using UnityEngine;

namespace src.util
{
    public class Broker
    {
        public static AbstractActor GetActor(ActorController controller)
        {
            switch (controller.Actor)
            {
                case PawnActor p:
                    return new PawnActor();
                default:
                    Debug.Log("Unable to instantiate Actor [" + controller.name + "]");
                    return null;
            }
        }
    }
}