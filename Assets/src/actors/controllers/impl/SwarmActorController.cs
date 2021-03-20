using System.Collections;
using src.ai;
using src.ai.swarm;
using src.util;
using UnityEngine;

namespace src.actors.controllers.impl
{
    public class SwarmActorController : AbstractActorController
    {
        PawnActorController player;
        GameObject target;

        SwarmService swarmService;

        Coroutine currentRoutine;

        bool inHeatZone = false;
        
        /*===============================
         *  Properties
         ==============================*/
        public bool InHeatZone
        {
            get => inHeatZone;
            set
            {
                HasLeftHeatZone();
                inHeatZone = value;
            }
        }

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
        void HasLeftHeatZone()
        {
            if (stateMachine.currentState == State.Locate
            || stateMachine.currentState == State.Attack)
            {
                
            }
        }
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
            while (target is null)
            {
                yield return null;
                target = swarmService.GetTarget(this);
            }
            
            
        }
        
        IEnumerator AttackRoutine()
        {
            //TODO Attack (recursive if possible)
            yield break;
        }
    }
}