using System.Collections.Generic;

namespace src.level
{
    public static class WaveRepository
    {
        public static List<Wave> GetAll()
        {
            var list = new List<Wave> {WaveOne};
            return list;
        }
        
        static Wave WaveOne 
            => Wave.Builder(0)
                   .NumberOfEntities(1)
                   .Batches(1)
                   .PlayerTargetWeight(0f)
                   .Build();
        static Wave WaveTwo 
            => Wave.Builder(1)
                .NumberOfEntities(4)
                .Batches(2)
                .PlayerTargetWeight(0.5f)
                .Build();
        static Wave WaveThree
            => Wave.Builder(2)
                .NumberOfEntities(3)
                .Batches(5)
                .PlayerTargetWeight(0.1f)
                .Build();
        static Wave WaveFour 
            => Wave.Builder(3)
                .NumberOfEntities(1)
                .Batches(1)
                .AttackPlayer(true)
                .Build();
        static Wave WaveFive 
            => Wave.Builder(4)
                .NumberOfEntities(1)
                .Batches(1)
                .AttackPlayer(true)
                .Build();
    }
}