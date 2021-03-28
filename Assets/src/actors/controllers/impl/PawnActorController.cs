using System;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class PawnActorController : AbstractActorController
    {
        /*===============================
         *  Lifecycle
         ==============================*/
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) sprite.Load();
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}