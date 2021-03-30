using System.Collections;
using System.Collections.Generic;
using System.Linq;
using src.actors.controllers.impl;
using src.actors.model;
using src.level;
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
        public event MembersRemaining Remaining = delegate { };


        /*===============================
        *  Fields
        ==============================*/
        readonly HashSet<SwarmActorController> attackingPlayer 
            = new HashSet<SwarmActorController>();
        readonly HashSet<SwarmActorController> attackingInfrastructure 
            = new HashSet<SwarmActorController>();
        

        PawnActorController player;

        GameObject swarmPrototype;

        Wave wave;


        /*===============================
         *  Initialization
         ==============================*/
        public SwarmService()
        {
            Environment.GameService.ReadiedWave += SpawnWave;
            Environment.PlayerService.Player    += controller => player = controller;
        }

        public void Init()
        {
            swarmPrototype =
                Resources.Load(Environment.RESOURCE_SWARM_MEMBER)
                    as GameObject;
        }


        
        /*===============================
         *  Spawning
         ==============================*/
        void SpawnWave(Wave wave)
        {
            this.wave = wave;
            StartSpawning();
        }

        /*===============================
         *  Handling
         ==============================*/
        void StartSpawning()
        {
            Environment.Log(GetType(), "Spawning wave::" + wave.waveNumber);

            var index = 0;
            var infrastructureTarget 
                = Environment.BuilderService.GetInfrastructureTarget();
            while (index < wave.batches)
            {
                var toAttackPlayer = ShouldAttackPlayer();
                var spawnPoint = GetSpawnPoint(
                    toAttackPlayer
                        ? player.transform.position
                        : infrastructureTarget.transform.position, 
                    toAttackPlayer);
                var target = toAttackPlayer
                    ? player.gameObject
                    : infrastructureTarget;

                Environment.GameService.Mono.StartCoroutine(
                    SpawnRoutine(spawnPoint, target.transform, toAttackPlayer));
                index++;
            }
        }


        /*===============================
         *  Getters & Setters
         ==============================*/
        public void Remove(SwarmActorController controller)
        {
            if (attackingPlayer.Contains(controller)) 
                attackingPlayer.Remove(controller);
            else if (attackingInfrastructure.Contains(controller)) 
                attackingInfrastructure.Remove(controller);

            attackingPlayer.RemoveWhere(c
                => attackingInfrastructure.Contains(c));

            Environment.LootService.DropLoot(controller);
            
            Remaining(attackingPlayer.Count 
                      + attackingInfrastructure.Count);
        }

        
        
        /*===============================
         *  Routines
         ==============================*/
        IEnumerator SpawnRoutine(Vector3 spawnPoint, 
                                 Component target,  
                                 bool toAttackPlayer)
        {
            var store = new List<GameObject>();
            
            var multiplier = Mathf.RoundToInt(
                Random.Range(
                    Environment.SWARM_WAVE_MULTIPLIER_MIN, 
                    Environment.SWARM_WAVE_MULTIPLIER_MAX));
            var numberToSpawn = wave.numberOfEntities * multiplier;
            
            const float radius = Environment.SWARM_SPAWN_RADIUS;

            while (store.Count < numberToSpawn)
            {
                var spawnPosition = Vector3.zero;

                const int maxFrames = 5;
                var frame = 0;
                while (spawnPosition.Equals(Vector3.zero))
                {
                    var tries = 0;
                    while (tries < 3)
                    {
                        var randPoint2D = Random.insideUnitCircle.normalized * radius;
                        var randPoint = new Vector3(randPoint2D.x, 0, randPoint2D.y);
                        randPoint += spawnPoint;

                        Collider[] coll = {};
                        Physics.OverlapSphereNonAlloc(randPoint, 3f, coll);

                        coll = coll
                            .Where(c => Environment.noOverlapTags.Contains(c.tag))
                            .ToArray();

                        if (coll.Length == 0)
                        {
                            spawnPosition = randPoint;
                            break;
                        }

                        tries++;
                    }
                    
                    if (frame >= maxFrames) break;

                    frame++;
                    yield return null;
                }
                
                if (spawnPosition == Vector3.zero) continue;

                var spawn = Object.Instantiate(
                    swarmPrototype, 
                    spawnPosition, 
                    new Quaternion());

                if (spawn) store.Add(spawn);
                yield return null;
            }

            foreach (var swarm in store)
            {
                if (swarm
                    .TryGetComponent<SwarmActorController>(out var sac))
                {
                    sac.Ready(target.gameObject, toAttackPlayer);
                    if (toAttackPlayer) attackingPlayer.Add(sac);
                    else attackingInfrastructure.Add(sac);
                }
            }

            Remaining(attackingInfrastructure.Count + attackingPlayer.Count);
        }

        
        
        /*===============================
         *  Utility
         ==============================*/
        Vector3 GetSpawnPoint(Vector3 target, bool attackingPlayer)
        {
            var spawnRadius 
                = Random.Range(Environment.SWARM_SPAWN_NEAR, Environment.SWARM_SPAWN_FAR);

            var randPoint2D = Random.insideUnitCircle.normalized;
            var randPoint = new Vector3(randPoint2D.x, 0, randPoint2D.y);
            var direction = randPoint - Vector3.zero;

            if (attackingPlayer || player is null)
            {
                return target + (direction * spawnRadius);
            }
            
            var playerPoint = player.transform.position;
            var targetPlayerDirection = (target - playerPoint).normalized;
            var targetPlayerDistance = Vector3.Distance(target, playerPoint);

            if (targetPlayerDistance < Environment.SWARM_SPAWN_NEAR + spawnRadius)
            {
                var undershoot = (Environment.SWARM_SPAWN_NEAR + spawnRadius) - targetPlayerDistance;
                target += targetPlayerDirection * undershoot;
            }


            return target + (direction * spawnRadius);
        }

        bool ShouldAttackPlayer()
        {
            var playerFocusMax = 
                Mathf.Round(wave.batches * Environment.SWARM_WAVE_PLAYER_MAX);
            var rollValue = Random.Range(0f, 1f);
            
            return rollValue <= wave.playerTargetWeight
                   && attackingPlayer.Count < playerFocusMax;
        }
    }
}