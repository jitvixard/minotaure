using System;
using System.Collections;
using src.actors.controllers.impl;
using src.scripting;
using src.util;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.services.impl
{
    public class GameService : IService
    {
        public bool IsRunning => currentRoutine != null;
        
        GameBehaviour gameBehaviour;

        PawnActorController player;

        Coroutine currentRoutine;
        
        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.PlayerService.Player += controller => player = controller;
            
            gameBehaviour = GameObject
                .FindGameObjectWithTag(Environment.TAG_MAIN_CAMERA)
                .GetComponent<GameBehaviour>();
            
            currentRoutine = gameBehaviour.StartCoroutine(StartRoutine());
        }

        /*===============================
        *  Routines
        ==============================*/
        IEnumerator StartRoutine()
        {
            IOHandler.Log(GetType(), "Start routine beginning");
            while (player is null) yield return null;
            IOHandler.Log(GetType(), "Start routine ending");
        }



    }
}