using System;
using System.Collections;
using src.card.model;
using src.services.impl;
using UnityEngine;
using UnityEngine.EventSystems;
using Environment = src.util.Environment;

namespace src.card.behaviours
{
    public abstract class CardBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public Card Card
        {
            set
            {
                if (card is null)
                {
                    value.behaviour = this;
                    card           = value;
                }
            }
        }
        public bool IsRunning => routine != null;

        CardService cardService;

        Card        card;
        Coroutine   routine;

        /**************** Card Behaviour ****************/
        protected abstract IEnumerator BehaviourRoutine();


        /*===============================
        *  Lifecycle
        ==============================*/
        protected  virtual void Awake()
        {
            cardService = Environment.CardService;
        }

        public bool Play()
        {
            routine = StartCoroutine(BehaviourRoutine());
            return IsRunning;
        }

        public bool Stop()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                return true;
            }

            return false;
        }
        
        void OnDisable()
        {
            Stop();
        }

        void OnDestroy()
        {
            Stop();
        }
        
        /*===============================
        *  Handler
        ==============================*/
        public void OnPointerClick(PointerEventData eventData)
        {
            print("click");
            cardService.Focus(card);
        }
    }
}