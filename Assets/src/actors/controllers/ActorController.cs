using System.Collections;
using src.actors.model;
using src.ai;
using src.util;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

namespace src.actors.controllers
{
    public class ActorController : MonoBehaviour
    {
        //Properties ====================
        //actor data model
        public AbstractActor Actor { get; set; }
        //nav agent
        public NavMeshAgent Agent { get; set; }


        //Variables =====================
        //state machine
        AbstractStateMachine stateMachine;

        protected Coroutine moveRoutine; 

        
        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Actor = Broker.GetActor(this);
            stateMachine = StateMachineFactory.Get(Actor);
        }

        public void Move(Vector3 target)
        {
            moveRoutine = StartCoroutine(MoveRoutine(target));
        }

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
    }
}
