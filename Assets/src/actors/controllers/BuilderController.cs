using System;
using System.Collections;
using System.Linq;
using src.actors.controllers;
using src.actors.model;
using src.card.behaviours.impl;
using src.impl;
using src.model;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Environment = src.util.Environment;

public class BuilderController : AbstractActorController
{
    public delegate void BuildingComplete(BuilderController controller, GameObject building);
    public event BuildingComplete Built = delegate {  };

    Image[]           knots;
    ProceduralImage[] healthDots;

    GameObject beacon;

    GameObject prototypeTower;

    bool completed;
    

    /*===============================
    *  Lifecycle
    ==============================*/
    protected override void Awake()
    {
        //assigning services
        playerService = Environment.PlayerService;
        swarmService  = Environment.SwarmService;

        //set prototypes
        prototypeExplosion = Resources.Load(Environment.RESOURCE_EXPLOSION)
            as GameObject;
        prototypeTower = Resources.Load(Environment.RESOURCE_BUILDING)
            as GameObject;

        healthDots = GetComponentsInChildren<ProceduralImage>()
            .Where(img => img.name.Contains(Environment.UI_HEALTH_INDICATOR))
            .ToArray();
        
        knots = GetComponentsInChildren<Image>()
            .Where(img => img.name.Contains(Environment.UI_KNOT))
            .ToArray();

        beacon = Environment.BuilderService.NextBeacon;

        StartCoroutine(InitialRoutine());
    }

    public override void Die()
    {
        Instantiate(prototypeExplosion, transform.position, new Quaternion());
        Destroy(stateMachine);
        Destroy(gameObject);
    }

    public override void Damage(AbstractActorController actorController)
    {
        if (actor.health-- == 1)
        {
            Die();
            return;
        }
        
        var updatedHealth = new ProceduralImage[healthDots.Length - 1];
        Array.Copy(healthDots, 
            1,
            updatedHealth,
            0,
            healthDots.Length - 1);
        
        Destroy(healthDots[0]);
        healthDots = updatedHealth;
    }

    public void PlaceOnMesh()
    {
        agent         = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        stateMachine  = StateMachineFactory.Create(actor);
        actor         = ActorFactory.Create(this);

        foreach (var col in GetComponentsInChildren<Collider>()) 
            col.enabled = true;
        
        //all state machines are defaulted to idle
        stateMachine.CurrentState = State.Idle;
    }
    
    
    
    /*===============================
    *  Handlers
    ==============================*/
    public void Move()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(MoveRoutine());
    }

    public void Unload()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(UnloadRoutine());
    }
    
    
    
    /*===============================
    *  Routines
    ==============================*/
    IEnumerator InitialRoutine()
    {
        var endPosition = Environment.BUILDER_FLOAT_DISTANCE * Vector3.forward;
        var currentPosition = transform.position;

        var t = 0f;
        var interval = Environment.BUILDER_FLOAT_TIME;
        
        while (Vector3.Distance(endPosition, currentPosition) > 0.1f)
        {
            var newPostion = new Vector3(
                Mathf.Lerp(currentPosition.x, endPosition.x, t / interval),
                Mathf.Lerp(currentPosition.y, endPosition.y, t / interval),
                Mathf.Lerp(currentPosition.z, endPosition.z, t / interval)
            );

            t += Time.deltaTime;

            transform.position = newPostion;
            currentPosition    = newPostion;
            yield return null;
        }
        
        PlaceOnMesh();
    }

    IEnumerator MoveRoutine()
    {
        var targetPosition = beacon.transform.position;
        agent.isStopped = false;
        agent.SetDestination(targetPosition);

        while (Vector3.Distance(targetPosition, transform.position) 
               > Environment.BUILDER_UNLOAD_DISTANCE)
        {
            yield return null;
        }

        agent.SetDestination(transform.position);
        agent.isStopped = true;
        stateMachine.CurrentState = State.Stopped;
    }
    
    IEnumerator UnloadRoutine()
    {
        var i = 0;
        while (i < knots.Length)
        {
            var t = 0f;
            while (t < Environment.BUILDER_UNLOAD_TIME)
            {
                t += Time.deltaTime;
                yield return null;
            }

            if (knots.Length == healthDots.Length)
            {
                Destroy(healthDots[i]);
            }

            Destroy(knots[i++]);
        }

        Destroy(beacon.gameObject);
        
        var building = Instantiate(
            prototypeTower,
            beacon.transform.position,
            new Quaternion());

        Built(this, building);

        yield return null;

        completed = true;
        Die();
    }
    
}
