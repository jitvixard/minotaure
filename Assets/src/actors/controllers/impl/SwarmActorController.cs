using System.Collections;
using System.Diagnostics;
using src.ai;
using src.ai.swarm;
using src.interfaces;
using src.util;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        PawnActorController player;
        SwarmService swarmService;


        /*===============================
         *  Properties
         ==============================*/
        public GameObject Player
        {
            get => player.gameObject;
            set
            {
                if (value.TryGetComponent<PawnActorController>(out var pac))
                    if (pac.IsSelected)
                        player = pac;
            }
        }

        
        /*===============================
         *  Instantiation
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            swarmService = Environment.SwarmService.Add(this);
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
                if (!(io.SelectedPawn is null))
                    target = io.SelectedPawn.gameObject;
        }
    }
}