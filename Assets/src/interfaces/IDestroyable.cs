using System.Collections;
using src.actors.controllers;
using UnityEngine;

namespace src.interfaces
{
    public interface IDestroyable
    {
        bool Damage(AbstractActorController actorController);
        int Health();
        float ExtraOffset();
        Transform GetTransform();
    }
}