namespace src.scripting.level
{
    public class Wave
    {
        public readonly int number;

        public readonly int batches;
        public readonly int numberOfEntities;

        public readonly int guaranteedDrops;
        
        public readonly bool attackPlayer;

        public Wave(
            int number,
            int batches,
            int numberOfEntities,
            int guaranteedDrops,
            bool attackPlayer)
        {
            this.number = number;
            this.batches = batches;
            this.numberOfEntities = numberOfEntities;
            this.guaranteedDrops = guaranteedDrops;
            this.attackPlayer = attackPlayer;
        }

        public static Wave Blank => new Wave(0, 1, 1, 1, true);
    }
}