using System.Collections.Generic;
using src.actors.controllers.impl;
using src.io;

namespace src.ai.swarm
{
    public class SwarmService
    {
        public IOHandler IO { get; set; }

        readonly HashSet<SwarmActorController> activeMembers = new HashSet<SwarmActorController>();
        
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