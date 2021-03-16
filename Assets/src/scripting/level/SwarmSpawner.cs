using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

namespace src.scripting.level
{
    public class SwarmSpawner : MonoBehaviour
    {
        Queue<Wave> waveOrder = new Queue<Wave>(); //TODO populate from JSON
        Vector3[] spawnPoints;

        GameObject swarmMember;

        Coroutine spawnRoutine;
        
        void Awake()
        {
            //TODO mocked set-up (remove)
            waveOrder.Enqueue(new Wave(3));
            
            //getting spawn points
            var children = GetComponentsInChildren<Transform>();
            spawnPoints = new Vector3[children.Length];
            var index = 0;
            foreach (var t in GetComponentsInChildren<Transform>())
            {
                if (t.name.Equals(name)) continue;
                spawnPoints[index++] = t.position;
            }
            
            swarmMember = Resources.Load(Environment.RESOURCE_SWARM_MEMBER) as GameObject;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) spawnRoutine = StartCoroutine(Spawning());
        }

        IEnumerator Spawning()
        {
            var wave = waveOrder.Dequeue();
            var index = 0;
            while (index < wave.numberOfEntities)
            {
                var spawnIndex = Random.Range(0, spawnPoints.Length - 1);
                
                var spawnDelay = Random.Range(Environment.SPAWN_DELAY_LOWER, Environment.SPAWN_DELAY_UPPER);
                spawnDelay *= 1000; //spawn delay set to millis
                var watch = new Stopwatch();
                watch.Start();
                
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

        void Spawn(Vector3 position)
        {
            var freshSpawn = Instantiate(swarmMember, position, new Quaternion());
            freshSpawn.name = Environment.SWARM_MEMBER + freshSpawn.GetInstanceID();
            freshSpawn.GetComponent<NavMeshAgent>().SetDestination(Vector3.zero);
        }
    }
}
