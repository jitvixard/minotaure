using src.actors.controllers;

namespace src.actors.model
{
	public class BuilderActor : AbstractActor
	{
		public BuilderActor(AbstractActorController controller) : base(controller)
		{
			health = 4;
		}
	}
}