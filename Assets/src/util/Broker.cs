using src.actors.controllers;
using src.actors.model;
using UnityEngine;

namespace src.util
{
    public class Broker
    {
        public static AbstractActor GetActor(ActorController controller)
        {
            Debug.Log("Defaulting Actor [" + controller.name + "]");
            return new PawnActor(controller);
            /*switch (controller.Actor)
            {
                case PawnActor p:
                    return new PawnActor(controller);
                default:
                    //Debug.Log("Unable to instantiate Actor [" + controller.name + "]");
                    Debug.Log("Defaulting Actor [" + controller.name + "]");
                    return new PawnActor(controller);
            }*/
        }
    }
}