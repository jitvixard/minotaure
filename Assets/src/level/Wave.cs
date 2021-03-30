namespace src.level
{
    public class Wave
    {
        public static WaveBuilder Builder(int waveNumber) => new WaveBuilder(waveNumber);
        
        public readonly int waveNumber;
        public readonly int batches;
        public readonly int numberOfEntities;

        public readonly float playerTargetWeight;

        public Wave(
            int waveNumber,
            int batches,
            int numberOfEntities,
            float playerTargetWeight)
        {
            this.waveNumber         = waveNumber;
            this.batches            = batches;
            this.numberOfEntities   = numberOfEntities;
            this.playerTargetWeight = playerTargetWeight;
        }
    }
    
    public class WaveBuilder
    {
        readonly int waveNumber;
        int          batches = 1;
        int          numberOfEntities;

        bool  attackPlayer = true;
        float playerTargetWeight;

        public WaveBuilder(int waveNumber)
        {
            this.waveNumber = waveNumber;
        }

        public WaveBuilder NumberOfEntities(int numberOfEntities)
        {
            this.numberOfEntities = numberOfEntities;
            return this;
        }
        
        public WaveBuilder Batches(int batches)
        {
            this.batches = batches;
            return this;
        }

        public WaveBuilder AttackPlayer(bool attackPlayer)
        {
            this.attackPlayer = attackPlayer;
            return this;
        }

        public WaveBuilder PlayerTargetWeight(float playerTargetWeight)
        {
            this.playerTargetWeight = playerTargetWeight;
            return this;
        } 

        public Wave Build()
        {
            if (numberOfEntities == 0) numberOfEntities = waveNumber + 1;
            if (attackPlayer) playerTargetWeight = 1f;
            
            return new Wave(waveNumber, batches, numberOfEntities, playerTargetWeight);
        }
    }
}