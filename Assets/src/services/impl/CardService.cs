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

        CardTabHandler cardTabHandler;

        
        Card[] possibleCards;
        //key: int -> wave number
        //value: Wave -> possible cards to spawn 
        readonly Card[][] cardBatches= CardRepository.AllBatches;

        
        Card selectedCard;

        
        
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
        
        public void AddCard(Card card)
        {
            Environment.Log(GetType(), "adding card");
            if (cardTabHandler.AddCard(card)) 
                activeCards.Add(card);
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