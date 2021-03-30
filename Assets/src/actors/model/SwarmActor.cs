using src.actors.controllers;
using src.level;

namespace src.actors.model
{
    public class SwarmActor : AbstractActor
    {
        public Wave wave;

        public SwarmActor(AbstractActorController controller) : base(controller)
        {
        }
    }
}