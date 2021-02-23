using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Environment = src.util.Environment;

namespace src.player
{
    public class PlayerController : MonoBehaviour
    {
        NavMeshAgent agent;

        //navigation
        Vector3 target;
        
        //routines
        Coroutine movementRoutine;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(GameObject tile)
        {
            target = tile.transform.position;
            movementRoutine = StartCoroutine(MoveRoutine());
        }

        IEnumerator MoveRoutine()
        {
            yield return null;
            agent.SetDestination(target);

            while (agent.remainingDistance <= 0) yield return null;
            
            while (agent.remainingDistance >= Environment.STOPPING_DISTANCE)
            {
                print(agent.remainingDistance);
                yield return null;
            }

            print("stopping");
            agent.SetDestination(transform.position);
            movementRoutine = null;
        } 
    }
}