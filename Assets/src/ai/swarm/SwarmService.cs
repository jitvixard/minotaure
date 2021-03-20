using System.Collections.Generic;
using System.Linq;
using src.actors.controllers.impl;
using src.io;
using src.util;
using UnityEngine;

namespace src.ai.swarm
{
    public class SwarmService
    {
        public IOHandler IO { get; set; }
        public HeatZone HeatZone { get; set; }

        readonly HashSet<SwarmActorController> activeMembers = new HashSet<SwarmActorController>();
        readonly HashSet<SwarmActorController> available = new HashSet<SwarmActorController>();

        readonly Dictionary<string, List<GameObject>> targetedPoi = new Dictionary<string, List<GameObject>>();


        /*===============================
         *  Handling
         ==============================*/
        public GameObject GetTarget(SwarmActorController controller)
        {
            available.Add(controller);

            var colliders = new Collider[] { }; //colliders within 'vision'
            Physics.OverlapSphereNonAlloc(
                controller.transform.position,
                Environment.SWARM_VISION_RANGE,
                colliders);

            return colliders //points of interest in 'vision'
                .Select(c => c.gameObject)
                .Where(g =>
                {
                    if (!Environment.PoiTags.Contains(g.tag)) return false;
                    if (targetedPoi.TryGetValue(g.name, out var attackers))
                    {
                        if (attackers.Count >= Environment.SWARM_MAX_ATTACKERS) return false;
                    }
                    return true;
                }) //sorted to find poi that do not exceed max attacker threshold
                .OrderBy(g =>
                {
                    var weight = 0f;
                    if (targetedPoi.TryGetValue(g.name, out var attackers))
                        weight += attackers.Count;
                    weight += Vector3.Distance(g.transform.position, controller.transform.position);
                    return weight;
                }) //sorted by a weight (distance + attackers)
                .First();
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
    }
}