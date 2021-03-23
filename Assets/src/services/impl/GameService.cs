using System;
using System.Collections;
using src.actors.controllers.impl;
using src.handlers;
using src.scripting;
using src.scripting.level;
using src.util;
using UnityEngine;
using Environment = src.util.Environment;
using Random = UnityEngine.Random;

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
        public MonoBehaviour Mono => gameBehaviour;
        
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
            this.wave = wave;
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            StartCoroutine(GraceRoutine());
        }
        
        
        /*===============================
        *  Routines
        ==============================*/
        IEnumerator StartRoutine()
        {
            while (player is null) yield return null;
            Environment.WaveService.Start();
        }
        
        IEnumerator GraceRoutine()
        {
            var interval = Random.Range(
                Environment.SPAWN_INTERVAL_LOWER,
                Environment.SPAWN_INTERVAL_UPPER);
            var t = 0f;
            
            IOHandler.Log(
                GetType(),
                "Grace period of " + interval + " starting.");
            
            while (t < interval && GracePeriodShouldEnd())
            {
                t += Time.deltaTime;
                yield return null;
            }
            
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