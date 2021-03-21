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
        /*===============================
        *  Fields
        ==============================*/
        readonly HashSet<SwarmActorController> activeMembers = 
            new HashSet<SwarmActorController>();
        readonly HashSet<SwarmActorController> available = 
            new HashSet<SwarmActorController>();

        readonly Dictionary<string, List<SwarmActorController>> targetedPoi = 
            new Dictionary<string, List<SwarmActorController>>();

        GameObject player;

        int attackRate = 1000;
        int waveNumber = 1;
        
        /*===============================
         *  Properties
         ==============================*/
        public IOHandler IO { get; set; }
        public GameObject Player
        {
            get => player;
            set
            {
                if (value.TryGetComponent<PawnActorController>(out var pac))
                    if (pac.IsSelected)
                    {
                        player = value;
                        foreach (var sac in activeMembers)
                            sac.Player = value;
                    }
            }
        }
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
        public bool NextWave()
        {
            SpawnWave();
            waveNumber++;
            return true;
        }

        void SpawnWave()
        {
            
        }
        
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