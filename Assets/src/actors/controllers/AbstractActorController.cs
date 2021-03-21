using System;
using System.Collections;
using System.Diagnostics;
using src.actors.handlers;
using src.actors.model;
using src.impl;
using src.interfaces;
using src.model;
using src.services;
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

        protected PlayerService playerService;
        protected SwarmService swarmService;

        //tracking
        protected GameObject target;
        //state machine
        protected AbstractStateMachine stateMachine;
        //ui
        protected SpriteHandler sprite;
        
        //routines
        protected Coroutine currentRoutine;
        Coroutine idleRoutine;
        
        //status
        bool inHeatZone;
        bool selected;
        
        
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
                HasLeftHeatZone();
                inHeatZone = value;
            }
        }
        public bool IsSelected => selected;

        public bool InRange
        {
            get
            {
                if (target == null) return false;
                return Vector3.Distance(
                           target.transform.position,
                           transform.position)
                       <= Environment.ATTACK_RANGE;
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
            sprite = new SpriteHandler(this);
            //assigning services
            playerService = Environment.PlayerService;
            swarmService = Environment.SwarmService;
        }

        public abstract void Die();

        void OnDestroy()
        {
            Die();
        }

        /*===============================
         *  Interaction
         ==============================*/
        public AbstractActorController Select(bool selected)
        {
            this.selected = selected;
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

        public void Attack()
        {
            if (!(currentRoutine is null)) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(AttackRoutine());
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
            {
                stateMachine.CurrentState = State.Idle;
            }
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

            while (Vector3.Distance(target, transform.position) > Environment.STOPPING_DISTANCE)
            {
                yield return null;
            }

            agent.SetDestination(transform.position);
            actor.moving = false;
            currentRoutine = null;
        }
        
        protected IEnumerator SeekRoutine(Transform target)
        {
            actor.moving = true;
            agent.SetDestination(target.position);
            
            while (Vector3.Distance(target.position, transform.position) > Environment.STOPPING_DISTANCE)
            {
                yield return null;
            }

            agent.SetDestination(transform.position);
            actor.moving = false;
            currentRoutine = null;
        }

        protected IEnumerator AttackRoutine()
        {
            var watch = Stopwatch.StartNew();
            var targetController = target.GetComponent<AbstractActorController>();
            var targetTransform = target.transform;
            
            var destroyable = targetController.Destroyable();
            if (destroyable is null)
            {
                currentRoutine = null;
                stateMachine.CurrentState = State.Idle;
                yield break;
            }

            while (destroyable != null && targetController.actor.health > 0)
            {
                while (watch.ElapsedMilliseconds < actor.attackRate)
                {
                    agent.SetDestination(targetTransform.position);
                    yield return null;
                }
                destroyable = destroyable.Damage(actor.damage);
                if (targetController.actor.health <= 0) break;
            }

            agent.SetDestination(transform.position);
            Target = null;
            currentRoutine = null;
            stateMachine.CurrentState = State.Idle;
        }

        /*===============================
         *  Destroyable Interface
         ==============================*/
        //TODO Add Damage Animations
        public IDestroyable Damage(int damage)
        {
            var health = actor.health;
            health -= damage;
            actor.health = health;
            if (health <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(DestroyRoutine());
                return null;
            }
            return this;
        }

        public IDestroyable Heal(int damage)
        {
            actor.health += damage;
            return this;
        }

        public IDestroyable Destroyable()
        {
            return this;
        }

        public IEnumerator DestroyRoutine()
        {
            if (stateMachine) stateMachine.StopAllCoroutines();
            yield return null;
            Destroy(this);
        }
        
        
        /*===============================
         *  Information
         ==============================*/
        Vector3 GetLocationAroundUnit(Vector3 origin, int radius)
        {
            var randomPoint = (Random.insideUnitCircle * radius);
            return new Vector3(
                origin.x + randomPoint.x,
                origin.y,
                origin.z + randomPoint.y
            );
        }
    }
}


