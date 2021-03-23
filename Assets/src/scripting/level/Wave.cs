namespace src.scripting.level
{
    public class Wave
    {
        public readonly int number;
        public readonly int numberOfEntities;
        
        public readonly bool attackPlayer;

        public Wave(
            int number,
            int numberOfEntities,
            bool attackPlayer)
        {
            this.number = number;
            this.numberOfEntities = numberOfEntities;
            this.attackPlayer = attackPlayer;
        }
    }
}