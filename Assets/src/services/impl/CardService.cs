using System.Collections.Generic;
using src.card.model;
using src.handlers.ui;
using src.level;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class CardService : IService
    {
        /*************** Card Drops ***************/
        public delegate void SetCardsToDrop(Card[] cards);
        public event SetCardsToDrop CardDrops = delegate { };

        public delegate void SelectCard(Card card);
        public event SelectCard CardSelected = delegate {  };

        readonly HashSet<Card> activeCards
            = new HashSet<Card>();
        readonly Card[][] cardBatches 
            = CardRepository.AllBatches;
        
        Card[] possibleCards;
        Card   selectedCard;

        CardTabHandler cardTabHandler;

        public bool IsCardSelected => selectedCard != null;


        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.WaveService.NextWave
                += LoadNextCards;

            cardTabHandler = GameObject.FindWithTag(Environment.TAG_CARD_TAB)
                .GetComponent<CardTabHandler>();
        }
        
        
        
        /*===============================
        *  Handling
        ==============================*/
        public void Focus(Card card)
        {
            if (!activeCards.Contains(card))
            {
                Debug.Log("Cannot focus card: " + card.behaviour.name);
                return;
            }

            selectedCard = card;
            CardSelected(card);
        }

        public void ActivateCard(RaycastHit hit)
        {
            if (selectedCard.behaviour.Play(hit))
            {
                activeCards.Remove(selectedCard);
            }

            CardSelected(null);
            RemoveCard(selectedCard);
        }
        
        public void AddCard(Card card)
        {
            Environment.Log(GetType(), "adding card");
            if (cardTabHandler.AddCard(card)) 
                activeCards.Add(card);
        }

        void RemoveCard(Card card)
        {
            activeCards.Remove(card);
            cardTabHandler.RemoveCard(card);
        }
        
        void LoadNextCards(Wave wave)
        {
            possibleCards = 
                wave.waveNumber >= cardBatches.Length
                    ? GenerateCards(wave)
                    : cardBatches[wave.waveNumber];
            CardDrops(possibleCards);
        }

        
        
        /*===============================
        *  Utility
        ==============================*/
        Card[] GenerateCards(Wave wave)
        {
            //TODO generate cards post scripted levels
            return CardRepository.BatchOne;
        }
    }
}