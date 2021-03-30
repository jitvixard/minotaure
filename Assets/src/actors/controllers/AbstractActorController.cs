using System;
using src.actors.handlers.sprite;
using src.actors.model;
using src.interfaces;
using src.services.impl;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

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

        //ui
        protected SpriteHandler sprite;

        //services
        protected BuilderService builderService;
        protected PlayerService  playerService;
        protected SwarmService   swarmService;

        //tracking
        protected GameObject target;
        
        //prototypes
        protected GameObject prototypeExplosion;
        protected GameObject prototypeSplatter;


        /*===============================
         *  Properties
         ==============================*/
        public bool IsSelected { get; set; }


        /*===============================
         *  Lifecycle
         ==============================*/
        protected virtual void Awake()
        {
            //setting up actor
            agent = GetComponent<NavMeshAgent>();
            actor = ActorFactory.Create(this);

            //assigning services
            builderService = Environment.BuilderService;
            playerService  = Environment.PlayerService;
            swarmService   = Environment.SwarmService;

            //set prototypes
            prototypeExplosion = Resources.Load(Environment.RESOURCE_EXPLOSION)
                as GameObject;
            prototypeSplatter = Resources.Load(Environment.RESOURCE_SPLATTER) 
	            as GameObject;
        }

        protected abstract void OnDestroy();

        /*===============================
         *  Destroyable Interface
         ==============================*/
        public abstract void Die();

        public abstract bool Damage(AbstractActorController actorController);

        public abstract float ExtraOffset();

        public int Health() => actor.health;

        public Transform GetTransform() => transform;

        /*===============================
         *  Interaction
         ==============================*/
        public void Select(bool selected)
        {
            IsSelected = selected;
            sprite.Possess();
        }
    }
}