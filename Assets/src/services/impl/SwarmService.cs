using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using src.actors.controllers.impl;
using src.actors.model;
using src.model;
using src.scripting.level;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class SwarmService : IService
    {
        /*===============================
        *  Observable
        ==============================*/
        public delegate void MembersRemaining(int number);
        public event MembersRemaining Remaining = delegate {  };
        
        
        /*===============================
        *  Fields
        ==============================*/
        readonly HashSet<SwarmActorController> activeMembers = 
            new HashSet<SwarmActorController>();
        readonly HashSet<SwarmActorController> available = 
            new HashSet<SwarmActorController>();

        readonly List<Vector3> spawnPoints = 
            new List<Vector3>();

        readonly Dictionary<string, List<SwarmActorController>> targetedPoi = 
            new Dictionary<string, List<SwarmActorController>>();

        Coroutine spawnRoutine;
        
        GameObject swarmPrototype;
        GameObject otherAttackPoint;

        PawnActorController player;

        Wave wave;

        int attackRate = 1000;
        
        /*===============================
         *  Properties
         ==============================*/
        public HeatZone HeatZone { get; set; }
        public int AttackRate
        {
            get
            {
                var variation = attackRate / 100;
                variation *= 5;
                return attackRate + variation;
            }
        }


        /*===============================
         *  Initialization
         ==============================*/
        public SwarmService()
        {
            Environment.GameService.ReadiedWave += SpawnWave;
            Environment.PlayerService.Player += controller => player = controller;
        }
        
        public void Init()
        {
            swarmPrototype = 
                Resources.Load(Environment.RESOURCE_SWARM_MEMBER) 
                    as GameObject;
            
            var parentSpawner = GameObject
                .FindGameObjectWithTag(Environment.TAG_SPAWNER);
            spawnPoints.AddRange(
                parentSpawner
                    .GetComponentsInChildren<Transform>()
                    .Where(t => t.name != parentSpawner.name)
                    .Select(t => t.position)
                    .ToList());
        }
        
        
        /*===============================
         *  Spawning
         ==============================*/
        void SpawnWave(Wave wave)
        {
            IOHandler.Log(GetType(), 
                "Starting wave number [" + wave.number + "]");
            this.wave = wave;
            if (spawnRoutine != null)  return;
        }
        
        void Spawn(Vector3 position)
        {
            var freshSpawn =
                GameObject.Instantiate(
                    swarmPrototype,
                    position,
                    new Quaternion());
            freshSpawn.name = 
                Environment.SWARM_MEMBER 
                + freshSpawn.GetInstanceID();
            
            if (freshSpawn.TryGetComponent<SwarmActorController>(out var sac))
            {
                ((SwarmActor) sac.actor).wave = wave;
            }
            else
            {
                GameObject.Destroy(freshSpawn);
            }
        }
        
        /*===============================
         *  Handling
         ==============================*/
        public GameObject GetTarget(SwarmActorController controller)
        {
            return controller.CurrentState == State.Locate 
                ? GetAttackTarget(controller) 
                : GetSeekTarget(controller);
        }

        GameObject GetAttackTarget(SwarmActorController controller)
        {
            var colliderArr = new Collider[] { }; //colliders within 'vision'
            Physics.OverlapSphereNonAlloc(
                controller.transform.position,
                Environment.SWARM_VISION_RANGE,
                colliderArr);

            if (colliderArr.Length == 0) return null;
            var colliders = colliderArr.ToList();

            var target = colliders //points of interest in 'vision'
                .Select(c => c.gameObject)
                .Where(g =>
                {
                    if (!Environment.PoiTags.Contains(g.tag)) 
                        return false;
                    if (targetedPoi.TryGetValue(g.name, out var attackers))
                    {
                        if (attackers.Count >= Environment.SWARM_MAX_ATTACKERS) 
                            return false;
                    }
                    return true;
                }) //sorted to find poi that do not exceed max attacker threshold
                .OrderBy(g =>
                {
                    var weight = 0f;
                    
                    if (targetedPoi.TryGetValue(g.name, out var attackers))
                        weight += attackers.Count;
                    
                    return weight 
                           + Vector3.Distance(
                               g.transform.position,
                               controller.transform.position);
                }) //sorted by a weight (distance + attackers)
                .First();

            if (target is null) return null;
            if (!targetedPoi.ContainsKey(target.name)) 
                targetedPoi.Add(target.name, new List<SwarmActorController>());
            
            targetedPoi[target.name].Add(controller);

            return target;
        }

        GameObject GetSeekTarget(SwarmActorController controller)
        {
            if (controller.actor is SwarmActor a)
            {
                if (a.wave.attackPlayer) 
                    if (player) return player.gameObject;
                
            }
            return otherAttackPoint ? otherAttackPoint : GetOtherAttackPoint();
        }
        
         
        /*===============================
         *  Getters & Setters
         ==============================*/
        public SwarmService Add(SwarmActorController controller)
        {
            activeMembers.Add(controller); 
            Remaining(activeMembers.Count);
            return this;
        }
        
        public void Remove(SwarmActorController controller)
        {
            var success = activeMembers.Remove(controller);
            if (!success) return;
            Remaining(activeMembers.Count);
        }

        GameObject GetOtherAttackPoint()
        {
            //TODO set other attack point
            return otherAttackPoint;
        }
        
        /*===============================
         *  Routines
         ==============================*/
        IEnumerator Spawning()
        {
            var index = 0;
            while (index < wave.numberOfEntities)
            {
                var spawnIndex = Random.Range(0, spawnPoints.Count - 1);
                
                var spawnDelay = Random.Range(Environment.SPAWN_DELAY_LOWER, Environment.SPAWN_DELAY_UPPER);
                spawnDelay *= 1000; //spawn delay set to millis
                var watch = Stopwatch.StartNew();
                
                while (watch.ElapsedMilliseconds < spawnDelay)
                {
                    yield return null; //waiting for spawn delay to pass
                }

                var spawnPoint = spawnPoints[index];
                spawnPoint = new Vector3(
                    Random.Range(-Environment.SPAWN_MARGIN, Environment.SPAWN_MARGIN),
                    spawnPoint.y,
                    spawnPoint.z
                );

                Spawn(spawnPoint);
                index++;
            }
        }
    }
}