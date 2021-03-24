using src.scripting.level;
using src.util;

namespace src.services.impl
{
    public class WaveService : IService
    {
        public delegate void SetNextWave(Wave wave);
        public event SetNextWave NextWave = delegate {  };
        Wave currentWave;
        
        int waveNumber;
        int remaining;
        
        
        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.SwarmService.Remaining += CheckWaveState;
        }

        public void Start()
        {
            CheckWaveState(0);
        }
        
        
        /*===============================
        *  Handling
        ==============================*/
        void CheckWaveState(int remaining)
        {
            if (remaining == 0) WaveCompleted();
        }

        void WaveCompleted()
        {
            currentWave = CreateNewWave();
            NextWave(currentWave);
        }

        
        /*===============================
        *  Utility
        ==============================*/
        Wave CreateNewWave()
        {
            return Wave.Blank;
        }
    }
}