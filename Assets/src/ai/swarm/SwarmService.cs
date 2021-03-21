using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using src.actors.controllers.impl;
using src.actors.model;
using src.io;
using src.scripting.level;
using src.scripting.progression;
using src.util;
using UnityEngine;

namespace src.ai.swarm
{
    public class SwarmService
    {
        /*===============================
        *  Fields
        ==============================*/
        IOHandler io;
        MonoBehaviour monoBehaviour;
        ProgressionController progressionController;
        
        readonly HashSet<SwarmActorController> activeMembers = 
            new HashSet<SwarmActorController>();
        readonly HashSet<SwarmActorController> available = 
            new HashSet<SwarmActorController>();
        
        readonly HashSet<Wave> activeWaves = 
            new HashSet<Wave>();
        readonly Queue<Wave> waveOrder =
            new Queue<Wave>();
        Wave currentWave;
        
        readonly List<Vector3> spawnPoints = 
            new List<Vector3>();

        readonly Dictionary<string, List<SwarmActorController>> targetedPoi = 
            new Dictionary<string, List<SwarmActorController>>();

        Coroutine spawnRoutine;
        
        GameObject swarmPrototype;
        GameObject otherAttackPoint;

        bool enabled;

        int attackRate = 1000;
        int waveNumber = 1;
        
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
        public int WaveNumber => waveNumber;


        /*===============================
         *  Spawning
         ==============================*/
        public void Init(ProgressionController progression)
        {
            swarmPrototype = Environment.GetSwarmProtoype();
            
            if (!monoBehaviour) monoBehaviour = progression;
            
            waveOrder.Enqueue(new Wave(true, 1));

            var parentSpawner = GameObject.FindGameObjectWithTag(Environment.TAG_SPAWNER);
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
        public bool NextWave()
        {
            SpawnWave();
            waveNumber++;
            return true;
        }

        void SpawnWave()
        {
            spawnRoutine = monoBehaviour.StartCoroutine(Spawning());
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
                ((SwarmActor) sac.actor).wave = currentWave;
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
            var colliders = new Collider[] { }; //colliders within 'vision'
            Physics.OverlapSphereNonAlloc(
                controller.transform.position,
                Environment.SWARM_VISION_RANGE,
                colliders);

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
            var actor = controller.actor;
            if (actor != null)
                if (((SwarmActor) actor).wave.attackPlayer)
                    return Environment.PlayerService.Player.gameObject;
            return otherAttackPoint ? otherAttackPoint : GetOtherAttackPoint();
        }
        
         
        /*===============================
         *  Getters & Setters
         ==============================*/
        public SwarmService Add(SwarmActorController controller)
        {
            activeMembers.Add(controller); 
            return this;
        }
        
        public SwarmService Remove(SwarmActorController controller)
        {
            activeMembers.Remove(controller);
            return this;
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
            currentWave = waveOrder.Dequeue();
            var index = 0;
            while (index < currentWave.numberOfEntities)
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