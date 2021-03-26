using src.services.impl;

namespace src.scripting.level
{
    public class Wave
    {
        public readonly bool attackPlayer;

        public readonly int batches;

        public readonly int guaranteedDrops;
        public readonly int number;
        public readonly int numberOfEntities;

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

        Wave(WaveService waveService, Wave wave)
        {
        }

        public static Wave Blank => new Wave(0, 1, 1, 1, true);

        public static Wave ReadyWave(WaveService waveService, Wave wave)
        {
            return new Wave(waveService, wave);
        }
    }
}