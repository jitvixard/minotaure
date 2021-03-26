using src.scripting.level;
using src.util;

namespace src.services.impl
{
    public class WaveService : IService
    {
        /*************** Wave Observable ***************/
        public delegate void SetNextWave(Wave wave);
        public event SetNextWave NextWave = delegate { };

        Wave currentWave;

        /*===============================
        *  Fields
        ==============================*/
        Wave[] preparedWaves;
        int        remaining; //remaining swarm members
        int        setWaves;


        /*===============================
        *  Properties
        ==============================*/
        public int WaveNumber => waveNumber;
        public int WavesBeat  => wavesBeat;

        int waveNumber = -1;
        int wavesBeat  = -1;


        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.SwarmService.Remaining += CheckWaveState;
            LoadWaves();
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
            waveNumber++;
            wavesBeat++;
            currentWave = GetNextWave();
            NextWave(currentWave);
        }


        /*===============================
        *  Utility
        ==============================*/
        Wave GetNextWave()
        {
            return waveNumber < preparedWaves.Length 
                ? preparedWaves[waveNumber] 
                : GenerateWave();
        }

        Wave GenerateWave()
        {
            return null;
        }

        void LoadWaves() => preparedWaves 
            = Environment.GetListFromJson<WaveWrapper>().items;
    }
}