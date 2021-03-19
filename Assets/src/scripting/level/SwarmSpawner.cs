using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

namespace src.scripting.level
{
    public class SwarmSpawner : MonoBehaviour
    {
        readonly List<Vector3> spawnPoints = new List<Vector3>();
        Queue<Wave> waveOrder = new Queue<Wave>(); //TODO populate from JSON

        GameObject swarmMember;

        Coroutine spawnRoutine;
        
        void Awake()
        {
            //TODO mocked set-up (remove)
            waveOrder.Enqueue(new Wave(3));
            
            //getting spawn points
            var children = GetComponentsInChildren<Transform>()
                .Where(t => t.name != name)
                .Select(t => t.position);
            spawnPoints.AddRange(children);
            
            swarmMember = Resources.Load(Environment.RESOURCE_SWARM_MEMBER) as GameObject;
        }
        
        IEnumerator Spawning()
        {
            var wave = waveOrder.Dequeue();
            var index = 0;
            while (index < wave.numberOfEntities)
            {
                var spawnIndex = Random.Range(0, spawnPoints.Count - 1);
                
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
        }
    }
}
