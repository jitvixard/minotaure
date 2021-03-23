using System.Collections;
using src.util;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        PawnActorController player;
        
        /*===============================
         *  LifeCycle
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            playerService.Player += (controller => player = controller);
        }

        public override void Die()
        {
            swarmService.Remove(this);
            Destroy(gameObject);
        }


        /*===============================
         *  Primary Behaviour
         ==============================*/
        public void Locate()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(LocateRoutine());
        }

        /*===============================
         *  Routines
         ==============================*/
        IEnumerator LocateRoutine()
        {
            var attempts = 0;
            while (target is null 
                   && attempts++ < Environment.SWARM_MAX_LOCATE_ATTEMPTS)
            {
                yield return null;
                target = swarmService.GetTarget(this);  
            }

            if (target is null)
            {
                if (!(player is null))
                    target = player.gameObject;
            }
        }
    }
}