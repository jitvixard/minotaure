using System;
using src.actors.handlers.sprite;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class PawnActorController : AbstractActorController
    {
        /*===============================
         *  Lifecycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            sprite = new PawnSpriteHandler(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ((PawnSpriteHandler)sprite).Load();
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}