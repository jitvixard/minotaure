using System;
using System.Collections;
using src.actors.handlers.sprite;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.actors.controllers.impl
{
    public class PawnActorController : AbstractActorController
    {
        PawnSpriteHandler pawnSprite;
        
        GameObject prototypeShot;
        GameObject prototypeSplatter;

        Rigidbody rb;

        [SerializeField] float blastForce;

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

            prototypeShot = Resources.Load(Environment.RESOURCE_SHOT) 
                as GameObject;
            prototypeSplatter = Resources.Load(Environment.RESOURCE_SPLATTER) 
                as GameObject;

            rb = GetComponentInChildren<Rigidbody>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) rb.AddForce(Vector3.back * blastForce, ForceMode.Impulse);
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
            StartCoroutine(BleedRoutine(sac));
        }

        public void Heal()
        {
            actor.health += 1;
            pawnSprite.UpdateHealth();
        }

        
        
        /*===============================
         *  Routine
         ==============================*/
        IEnumerator BleedRoutine(SwarmActorController sac)
        {
            var origin = gameObject.transform.position;
            origin.y = 0;
            var target = sac.transform.position;
            target.y = 0;
            var direction = target - origin;
            direction = direction.normalized;

            var splatter = Instantiate(
                prototypeSplatter,
                transform.position, 
                Quaternion.LookRotation(direction));

            yield return null;

            rb.isKinematic = false;
            rb.AddForce(-direction * Environment.FX_BLAST_FORCE, ForceMode.Impulse);

            var t = 0f;
            var duration = Environment.FX_SPLATTER_TIME / 5;
            while (t < duration)
            {
                t += Time.deltaTime;
                yield return null;
            }

            rb.isKinematic = true;
            
            while (t < duration * 4)
            {
                t += Time.deltaTime;
                yield return null;
            }
            
            Destroy(splatter);
        }
    }
}