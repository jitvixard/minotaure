using src.actors.controllers.impl;
using src.handlers;
using src.scripting.level;
using src.util;

namespace src.services.impl
{
    public class WaveService : IService
    {
        public delegate void SetNextWave(Wave wave);
        public event SetNextWave NextWave = delegate {  };
        Wave currentWave;
        
        PawnActorController player;
        int waveNumber;
        int remaining;
        
        
        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.PlayerService.Player += controller => player = controller;
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