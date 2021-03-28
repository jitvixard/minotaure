namespace src.actors.controllers.impl
{
    public class PawnActorController : AbstractActorController
    {
        /*===============================
         *  Lifecycle
         ==============================*/
        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}