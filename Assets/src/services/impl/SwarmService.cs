using System.Collections;
using System.Collections.Generic;
using System.Linq;
using src.actors.controllers.impl;
using src.actors.model;
using src.level;
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


        /*===============================
        *  Fields
        ==============================*/
        readonly List<Vector3> spawnPoints =
            new List<Vector3>();

        readonly HashSet<SwarmActorController> attackingPlayer 
            = new HashSet<SwarmActorController>();
        readonly HashSet<SwarmActorController> attackingInfrastructure 
            = new HashSet<SwarmActorController>();
        

        PawnActorController player;
        BuilderController   builder;

        GameObject swarmPrototype;

        Wave wave;


        /*===============================
         *  Initialization
         ==============================*/
        public SwarmService()
        {
            Environment.BuilderService.Builder  += controller => builder = controller;
            Environment.GameService.ReadiedWave += SpawnWave;
            Environment.PlayerService.Player    += controller => player = controller;
        }

        /*===============================
         *  Properties
         ==============================*/
        public HeatZone HeatZone { get; set; }

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

        public event MembersRemaining Remaining = delegate { };


        /*===============================
         *  Spawning
         ==============================*/
        void SpawnWave(Wave wave)
        {
            this.wave = wave;

            Environment.GameService
                       .Mono.StartCoroutine(Spawning());
        }

        void Spawn(Vector3 position)
        {
            var freshSpawn =
                Object.Instantiate(
                    swarmPrototype,
                    position,
                    new Quaternion());

            freshSpawn.name = //setting name for readability
                Environment.SWARM_MEMBER
                + freshSpawn.GetInstanceID();

            if (freshSpawn.TryGetComponent<SwarmActorController>(out var sac))
            {
                ((SwarmActor) sac.actor).wave = wave;
            }
            else
            {
                Object.Destroy(freshSpawn);
                return;
            }

            Add(sac);
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
                             if (!Environment.pointsOfInterest.Contains(g.tag))
                                 return false;
                             if (targetedPoi.TryGetValue(g.name, out var attackers))
                                 if (attackers.Count >= Environment.SWARM_MAX_ATTACKERS)
                                     return false;
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
                if (a.wave.attackPlayer)
                    if (player)
                        return player.gameObject;

            return otherAttackPoint ? otherAttackPoint : GetOtherAttackPoint();
        }


        /*===============================
         *  Getters & Setters
         ==============================*/
        public void Add(SwarmActorController controller)
        {
            activeMembers.Add(controller);
            Remaining(activeMembers.Count);
        }

        public void Remove(SwarmActorController controller)
        {
            var success = activeMembers.Remove(controller);
            if (!success) return;

            Environment.LootService.DropLoot(controller);
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
            Environment.Log(GetType(), "Spawning wave::" + wave.waveNumber);
            var index = 0;
            while (index < wave.numberOfEntities)
            {
                var spawnDelay =
                    Random.Range(
                        Environment.SPAWN_DELAY_LOWER,
                        Environment.SPAWN_DELAY_UPPER);
                var t = 0f;

                while (t < spawnDelay)
                {
                    t += Time.deltaTime;
                    yield return null;
                }

                var spawnPoint = spawnPoints[index % spawnPoints.Count];
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