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
                   .Build();
    }
}