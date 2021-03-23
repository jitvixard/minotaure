using src.scripting.level;
using src.util;

namespace src.services.impl
{
    public class LootService : IService
    {
        public delegate void SetCard(Wave wave);
        public event SetCard Card = delegate {  };
        public delegate void SetScrap(int scrap);
        public event SetScrap Scrap = delegate {  };
        
        
        Wave currentWave;

        public void Init()
        {
            Environment.WaveService.NextWave += ReadyLoot;
        }

        void ReadyLoot(Wave wave)
        {
            currentWave = wave;
        }
    }
}