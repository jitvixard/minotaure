using System;
using System.Collections;
using src.actors.handlers;
using src.actors.model;
using src.ai;
using src.io;
using src.util;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

namespace src.actors.controllers
{
    public abstract class AbstractActorController : MonoBehaviour
    {
        //Properties ====================
        //actor data model
        [NonSerialized] public AbstractActor actor;
        //nav agent
        [NonSerialized] public NavMeshAgent agent;


        //Variables =====================
        // io
        protected IOHandler io;
        //state machine
        protected AbstractStateMachine stateMachine;
        //ui
        protected SpriteHandler sprite;
        //routines
        protected Coroutine moveRoutine; 
        //status
        protected bool selected = false;


        /*===============================
         *  Lifecycle
         ==============================*/
        protected virtual void Awake()
        {
            io = Camera.main.GetComponent<IOHandler>();
            agent = GetComponent<NavMeshAgent>();
            actor = ActorFactory.Create(this);
            stateMachine = StateMachineFactory.Get(actor);
            sprite = new SpriteHandler(this);
        }

        public abstract void Die();

        /*===============================
         *  Interaction
         ==============================*/
        public virtual AbstractActorController Select(bool selected)
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
        public virtual void Move(Vector3 target)
        {
            moveRoutine = StartCoroutine(MoveRoutine(target));
        }

        /*===============================
         *  Routines
         ==============================*/
        protected virtual IEnumerator MoveRoutine(Vector3 target)
        {
            actor.Moving = true;
            agent.SetDestination(target);

            while (Vector3.Distance(target, transform.position) > Environment.STOPPING_DISTANCE)
            {
                yield return null;
            }

            agent.SetDestination(transform.position);
            actor.Moving = false;
        }
        
        protected IEnumerator SeekRoutine(Transform target)
        {
            actor.Moving = true;
            agent.SetDestination(target.position);
            
            while (Vector3.Distance(target.position, transform.position) > Environment.STOPPING_DISTANCE)
            {
                yield return null;
            }

            agent.SetDestination(transform.position);
            actor.Moving = false;
        }

        /*===============================
         *  UI & Feedback
         ==============================*/
        public bool IsSelected()
        {
            return selected;
        }
    }
}


