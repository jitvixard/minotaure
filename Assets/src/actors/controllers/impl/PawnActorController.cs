namespace src.actors.controllers.impl
{
    public class PawnActorController : AbstractActorController
    {
        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}