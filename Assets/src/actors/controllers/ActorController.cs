using System;
using src.actors.model;
using src.ai;
using src.util;
using UnityEngine;
using UnityEngine.AI;

namespace src.controllers
{
    public class ActorController : MonoBehaviour
    {
        //Properties ====================
        //actor data model
        public AbstractActor Actor { get; set; }
        //nav agent
        NavMeshAgent Agent { get; set; }
        
        
        //Variables =====================
        //state machine
        AbstractStateMachine stateMachine;

        
        void Awake()
        {
            Actor = Broker.GetActor(this);
            Agent = GetComponent<NavMeshAgent>();

            stateMachine = StateMachineFactory.Get(Actor);
        }
    }
}
