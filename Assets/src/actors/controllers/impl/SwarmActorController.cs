using System.Collections;
using System.Diagnostics;
using src.interfaces;
using src.util;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    { 
        /*===============================
         *  LifeCycle
         ==============================*/
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
                var player = playerService.Player;
                if (!(playerService.Player is null))
                    target = player.gameObject;
            }
        }
    }
}