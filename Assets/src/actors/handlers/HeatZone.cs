using System.Collections;
using System.Collections.Generic;
using System.Linq;
using src.actors.controllers.impl;
using src.util;
using UnityEngine;

namespace src.scripting.level
{
    public class HeatZone : MonoBehaviour
    {
        readonly Dictionary<string, Coroutine> bufferRoutines    = new Dictionary<string, Coroutine>();
        readonly HashSet<SwarmActorController> membersInHeatZone = new HashSet<SwarmActorController>();
        readonly HashSet<GameObject>           pointsOfInterest  = new HashSet<GameObject>();


        /*===============================
    *  Unity Lifecycle
    ==============================*/
        void Awake()
        {
            Environment.SwarmService.HeatZone = this;
        }

        void OnDestroy()
        {
            StopAllCoroutines();
            foreach (var controller in membersInHeatZone) controller.InHeatZone = false;
            Environment.SwarmService.HeatZone = null;
        }

        /*===============================
        *  Events
        ==============================*/
        void OnTriggerEnter(Collider other)
        {
            var otherObj = other.gameObject;
            if (otherObj.TryGetComponent<SwarmActorController>(out var controller))
            {
                if (bufferRoutines.ContainsKey(controller.name))
                {
                    StopCoroutine(bufferRoutines[controller.name]);
                    bufferRoutines.Remove(controller.name);
                    return;
                }

                controller.InHeatZone = true;
                membersInHeatZone.Add(controller);
            }
            else if (Environment.PointsOfInterest.Contains(otherObj.tag))
            {
                pointsOfInterest.Add(otherObj);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Environment.TAG_SWARM)) return;
            if (other.gameObject.TryGetComponent<SwarmActorController>(out var controller))
                bufferRoutines.Add(controller.name, StartCoroutine(BufferRoutine(controller)));
        }

        /*===============================
        *  Routines
        ==============================*/
        IEnumerator BufferRoutine(SwarmActorController controller)
        {
            var t = 0f;
            while (t < Environment.HEAT_ZONE_DELAY)
            {
                t += Time.deltaTime;
                yield return null;
            }

            controller.InHeatZone = false;
            membersInHeatZone.Remove(controller);
        }
    }
}