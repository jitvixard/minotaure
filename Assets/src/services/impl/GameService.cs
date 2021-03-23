using System;
using System.Collections;
using src.actors.controllers.impl;
using src.scripting;
using src.scripting.level;
using src.util;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.services.impl
{
    public class GameService : IService
    {
        /*===============================
        *  Observables
        ==============================*/
        public delegate void WaveToStart(Wave wave);
        public event WaveToStart ReadiedWave = delegate {  };
        
        
        /*===============================
        *  Fields & Properties
        ==============================*/
        public bool IsRunning => currentRoutine != null;
        
        GameBehaviour gameBehaviour;

        PawnActorController player;

        Coroutine currentRoutine;

        Wave wave;
        
        
        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.PlayerService.Player += controller => player = controller;
            Environment.WaveService.NextWave += QueueNextWave;
            
            gameBehaviour = GameObject
                .FindGameObjectWithTag(Environment.TAG_MAIN_CAMERA)
                .GetComponent<GameBehaviour>();
            
            currentRoutine = gameBehaviour.StartCoroutine(StartRoutine());
        }

        /*===============================
        *  Handling
        ==============================*/
        void QueueNextWave(Wave wave)
        {
            IOHandler.Log(GetType(), "Queuing next wave");
            this.wave = wave;
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            StartCoroutine(GraceRoutine());
        }
        
        
        /*===============================
        *  Routines
        ==============================*/
        IEnumerator StartRoutine()
        {
            IOHandler.Log(GetType(), "Start routine beginning");
            while (player is null) yield return null;
            IOHandler.Log(GetType(), "Start routine ending");
            Environment.WaveService.Start();
        }
        
        IEnumerator GraceRoutine()
        {
            IOHandler.Log(GetType(), "Grace routine beginning");
            
            var t = 0f;
            while (t < Environment.SPAWN_INTERVAL 
                   && GracePeriodShouldEnd())
            {
                t += Time.deltaTime;
                yield return null;
            }
            
            IOHandler.Log(GetType(), "Grace routine ending");

            IOHandler.Log(GetType(), "Emitting Wave");
            ReadiedWave(wave);
        }
        
        
        /*===============================
        *  Utility
        ==============================*/
        bool GracePeriodShouldEnd()
        {
            return true;
        }
        
        /*===============================
        *  Wrappers
        ==============================*/
        Coroutine StartCoroutine(IEnumerator routine)
        {
            return gameBehaviour.StartCoroutine(routine);
        }
        
        void StopCoroutine(Coroutine routine)
        {
            gameBehaviour.StopCoroutine(routine);
        }
    }
}