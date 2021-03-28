using src.services.impl;

namespace src.level
{
    public class Wave
    {
        public static WaveBuilder Builder(int waveNumber) => new WaveBuilder(waveNumber);
        
        public readonly bool attackPlayer;

        public readonly int waveNumber;
        public readonly int numberOfEntities;

        public Wave(
            int waveNumber,
            int numberOfEntities,
            bool attackPlayer)
        {
            this.waveNumber = waveNumber;
            this.numberOfEntities = numberOfEntities;
            this.attackPlayer = attackPlayer;
        }
    }
    
    public class WaveBuilder
    {
        int waveNumber = 0;
        int numberOfEntities;

        bool attackPlayer = true;

        public WaveBuilder(int waveNumber)
        {
            this.waveNumber = waveNumber;
        }

        public WaveBuilder NumberOfEntities(int numberOfEntities)
        {
            this.numberOfEntities = numberOfEntities;
            return this;
        }

        public WaveBuilder AttackPlayer(bool attackPlayer)
        {
            this.attackPlayer = attackPlayer;
            return this;
        }

        public Wave Build()
        {
            if (numberOfEntities == 0) numberOfEntities = waveNumber + 1;
            return new Wave(waveNumber, numberOfEntities, attackPlayer);
        }
    }
}