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
        public AbstractActor Actor { get; set; }
        //nav agent
        public NavMeshAgent Agent { get; set; }


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


        void Awake()
        {
            io = Camera.main.GetComponent<IOHandler>();
            Agent = GetComponent<NavMeshAgent>();
            Actor = Broker.GetActor(this);
            stateMachine = StateMachineFactory.Get(Actor);
            sprite = new SpriteHandler(this);
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
            moveRoutine = StartCoroutine(MoveRoutine(target));
        }

        /*===============================
         *  Routines
         ==============================*/
        IEnumerator MoveRoutine(Vector3 target)
        {
            Actor.Moving = true;
            Agent.SetDestination(target);

            while (Vector3.Distance(target, transform.position) > Environment.STOPPING_DISTANCE)
            {
                yield return null;
            }

            Agent.SetDestination(transform.position);
            Actor.Moving = false;
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
