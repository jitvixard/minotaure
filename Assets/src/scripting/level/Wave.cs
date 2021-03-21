namespace src.scripting.level
{
    public class Wave
    {
        public readonly bool attackPlayer;
        public readonly int numberOfEntities;

        public Wave() {}
        public Wave(
            bool attackPlayer,
            int numberOfEntities)
        {
            this.attackPlayer = attackPlayer;
            this.numberOfEntities = numberOfEntities;
        }
    }
}