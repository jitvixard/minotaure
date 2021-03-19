namespace src.actors.controllers.impl
{
    public class PawnController : AbstractActorController
    {
        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}