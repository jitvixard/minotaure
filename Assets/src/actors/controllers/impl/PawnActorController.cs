using System;
using src.actors.handlers.sprite;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.actors.controllers.impl
{
    public class PawnActorController : AbstractActorController
    {
        PawnSpriteHandler pawnSprite;
        
        GameObject prototypeShot;

        bool canFire;

        /*===============================
         *  Lifecycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            
            pawnSprite        =  new PawnSpriteHandler(this);
            pawnSprite.Loaded += LoadCycle;
            
            pawnSprite.Load();
            
            sprite = pawnSprite;

            prototypeShot = Resources.Load(Environment.RESOURCE_SHOT) as GameObject;
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
        
        
        
        /*===============================
         *  Handling
         ==============================*/
        public void Fire(RaycastHit hit)
        {
            if (!canFire) return;
            
            pawnSprite.Fired();
            
            var shot = Instantiate(
                prototypeShot,
                transform.position,
                new Quaternion());
            shot.GetComponent<ShotBehaviour>().Launch(hit);

            canFire = false;
        }

        void LoadCycle(bool readied)
        {
            if (readied) canFire = true;
            else
            {
                pawnSprite.Load();
                canFire = false;
            }
        }
        
        
        
        /*===============================
         *  Incoming Event
         ==============================*/
        public void TakeDamage(SwarmActorController sac)
        {
            actor.health -= 1;
            pawnSprite.UpdateHealth();
        }

        public void Heal()
        {
            actor.health += 1;
            pawnSprite.UpdateHealth();
        }
    }
}