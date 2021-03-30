using System.Collections;
using src.actors.controllers;

namespace src.interfaces
{
    public interface IDestroyable
    {
        void Damage(AbstractActorController actorController);
        int Health();
        float ExtraOffset();
    }
}