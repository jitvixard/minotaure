using System.Collections;
using src.actors.controllers.impl;
using src.card.behaviours;
using src.level;
using src.scripting;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class GameService : IService
    {
        /*===============================
        *  Observables
        ==============================*/
        public delegate void WaveToStart(Wave wave);
        public event WaveToStart ReadiedWave = delegate { };

        Coroutine currentRoutine;

        GameBehaviour gameBehaviour;

        PawnActorController player;

        Wave wave;


        /*===============================
        *  Fields & Properties
        ==============================*/
        public bool          IsRunning => currentRoutine != null;
        public MonoBehaviour Mono      => gameBehaviour;


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
        public bool AddBehaviour(CardBehaviour cardBehaviour)
        {
            return true;
        }

        public void GameOver()
        {
            Application.Quit();
        }
        
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

            Environment.Log(
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