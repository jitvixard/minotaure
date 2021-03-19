using System.Collections;
using src.ai.swarm;
using src.util;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        PawnController player;
        GameObject target;

        SwarmService swarmService;

        Coroutine currentRoutine;
        
        /*===============================
         *  Instantiation
         ==============================*/
        protected override void Awake()
        {
            base.Awake();
            swarmService = Environment.SwarmService.Add(this);
            player = io.GetCurrentPawn();
        }

        public override void Die()
        {
            swarmService.Remove(this);
            Destroy(gameObject);
        }

        /*===============================
         *  Handling of Events & Stimuli
         ==============================*/
        //TODO Handle if Actor falls out of players range whilst Locating
        //TODO Handle destroying a target when attacking


        /*===============================
         *  Primary Behaviour
         ==============================*/
        public void Seek()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(SeekRoutine(player.transform));
        }
        
        public void Locate()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(LocateRoutine());
        }

        public void Attack()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(AttackRoutine());
        }
        
        /*===============================
         *  Routines
         ==============================*/
        IEnumerator LocateRoutine()
        {
            //TODO Coordinate with SwarmService to find next target
            yield break;
        }
        
        IEnumerator AttackRoutine()
        {
            //TODO Attack (recursive if possible)
            yield break;
        }
    }
}