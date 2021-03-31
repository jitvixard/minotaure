using System.Collections.Generic;
using src.card;
using src.card.model;
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
        
        
        
        CardHandler cardHandler;
        
        Card[] possibleCards;
        Card   selectedCard;

        public bool IsCardSelected => selectedCard != null;

        public bool CardSpaceAvailable => activeCards.Count < cardHandler.MaxCards();


        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.WaveService.NextWave    += LoadNextCards;
            Environment.LootService.DroppedCard += AddCard;
            
            cardHandler = GameObject.Find(Environment.UI_CARDS_DESK)
                .GetComponent<CardHandler>();
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
            if (selectedCard.behaviour.Play(hit) 
                && !selectedCard.behaviour.IsButton())
            {
                RemoveCard(selectedCard);
                Object.Destroy(selectedCard.behaviour.gameObject);
            }

            CardSelected(null);
            selectedCard = null;
            
            
        }

        void AddCard(Card card)
        {
            Environment.Log(GetType(), "adding card");
            if (activeCards.Add(card))
            {
                cardHandler.Add(card);
            }
        }

        void RemoveCard(Card card)
        {
            activeCards.Remove(card);
            cardHandler.Remove(card);
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