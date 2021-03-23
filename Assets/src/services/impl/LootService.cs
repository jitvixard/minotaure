using src.handlers;
using src.scripting.level;
using src.util;

namespace src.services.impl
{
    public class LootService : IService
    {
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