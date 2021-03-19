using src.actors.controllers;
using src.actors.model;
using UnityEngine;

namespace src.util
{
    public class Broker
    {
        public static AbstractActor GetActor(AbstractActorController controller)
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
}