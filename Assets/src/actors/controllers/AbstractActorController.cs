using System;
using System.Collections;
using System.Diagnostics;
using src.actors.controllers.impl;
using src.actors.handlers.sprite;
using src.actors.model;
using src.impl;
using src.interfaces;
using src.model;
using src.services.impl;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;
using Random = UnityEngine.Random;

namespace src.actors.controllers
{
    public abstract class AbstractActorController : MonoBehaviour, IDestroyable
    {
        /*===============================
         *  Fields
         ==============================*/
        //actor data model
        [NonSerialized] public AbstractActor actor;

        //nav agent
        [NonSerialized] public NavMeshAgent agent;

        //routines
        protected Coroutine currentRoutine;
        Coroutine           idleRoutine;

        //status
        bool inHeatZone;

        protected PlayerService playerService;

        //ui
        protected SpriteHandler sprite;

        //state machine
        protected AbstractStateMachine stateMachine;
        protected SwarmService         swarmService;

        //tracking
        protected GameObject target;


        /*===============================
         *  Properties
         ==============================*/
        public GameObject Target
        {
            get => target;
            set => target = value;
        }

        public bool InHeatZone
        {
            get => inHeatZone;
            set
            {
                if (!value) HasLeftHeatZone();
                inHeatZone = value;
                stateMachine.CheckState();
            }
        }

        public bool IsSelected { get; set; }

        public bool InRange
        {
            get
            {
                if (target is null) return false;
                return Vector3.Distance(
                           target.transform.position,
                           transform.position)
                       <= Environment.COMBAT_ATTACK_RANGE;
            }
        }

        public State CurrentState => stateMachine.CurrentState;


        /*===============================
         *  Lifecycle
         ==============================*/
        protected virtual void Awake()
        {
            //setting up actor
            agent = GetComponent<NavMeshAgent>();
            actor = ActorFactory.Create(this);
            stateMachine = StateMachineFactory.Create(actor);

            //assigning services
            playerService = Environment.PlayerService;
            swarmService = Environment.SwarmService;

            //all state machines are defaulted to idle
            stateMachine.CurrentState = State.Idle;
        }

        /*===============================
         *  Destroyable Interface
         ==============================*/
        //TODO Add Damage Animations
        public abstract void Die();

        public int Health()
        {
            return actor.health;
        }

        /*===============================
         *  Interaction
         ==============================*/
        public AbstractActorController Select(bool selected)
        {
            IsSelected = selected;
            if (selected) stateMachine.Stop();
            else stateMachine.Resume();
            sprite.Refresh();
            return this;
        }

        /*===============================
         *  Orders
         ==============================*/
        public void Move(Vector3 target)
        {
            currentRoutine = StartCoroutine(MoveRoutine(target));
        }

        public void Seek(Transform target)
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(SeekRoutine(target));
        }

        public void Idle()
        {
            if (idleRoutine != null) return;
            idleRoutine = StartCoroutine(IdleRoutine());
        }

        /*===============================
         *  Handling of Events & Stimuli
         ==============================*/
        protected void HasLeftHeatZone()
        {
            if (stateMachine.CurrentState == State.Locate
                || stateMachine.CurrentState == State.Attack)
                stateMachine.CurrentState = State.Idle;
        }
        //TODO Handle Destruction of Target (?)

        /*===============================
         *  Routines
         ==============================*/
        protected IEnumerator IdleRoutine()
        {
            var watch = new Stopwatch();
            var origin = transform.position;

            while (stateMachine.CurrentState == State.Idle)
            {
                watch.Start();
                var waitTime = Random.Range(Environment.IDLE_WAIT_LOWER, Environment.IDLE_WAIT_LOWER) * 1000;
                waitTime += watch.ElapsedMilliseconds;

                while (watch.ElapsedMilliseconds < waitTime) yield return null; //waiting

                Move(GetLocationAroundUnit(origin, Environment.IDLE_RANGE)); //move to random point

                while (actor.moving) yield return null; //waiting

                idleRoutine = null;
            }
        }

        protected IEnumerator MoveRoutine(Vector3 target)
        {
            actor.moving = true;
            agent.SetDestination(target);

            while (Vector3.Distance(target, transform.position) > Environment.STOPPING_DISTANCE) yield return null;

            agent.SetDestination(transform.position);
            actor.moving = false;
            currentRoutine = null;
        }

        protected IEnumerator SeekRoutine(Transform target)
        {
            actor.moving = true;
            agent.SetDestination(target.position);

            while (Vector3.Distance(target.position, transform.position) > Environment.SWARM_ATTACKING_RANGE)
            {
                agent.SetDestination(target.position);
                yield return null;
            }

            agent.SetDestination(transform.position);
            actor.moving              = false;
            currentRoutine            = null;
            stateMachine.CurrentState = State.Locate;
        }


        /*===============================
         *  Information
         ==============================*/
        Vector3 GetLocationAroundUnit(Vector3 origin, int radius)
        {
            var randomPoint = Random.insideUnitCircle * radius;
            return new Vector3(
                origin.x + randomPoint.x,
                origin.y,
                origin.z + randomPoint.y
            );
        }
    }
}