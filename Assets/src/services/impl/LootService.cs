using System.Collections.Generic;
using System.Linq;
using src.actors.controllers.impl;
using src.card.model;
using src.level;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class LootService : IService
    {
        /*************** Card Observable ***************/
        public delegate void SetCard(Card card);
        public event SetCard DroppedCard = delegate { };
        
        
        /*===============================
        *  Fields
        ==============================*/
        List<Card> availableCards  = new List<Card>();
        List<Card> guaranteedCards = new List<Card>();
        int guaranteedDrops;

        readonly float baseDropRate = Environment.LOOT_DROP_RATE;

        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            Environment.CardService.CardDrops += QueueLoot;
        }

        
        
        /*===============================
         *  Preparation
         ==============================*/
        void QueueLoot(Card[] cards)
        {
            availableCards.Clear();
            guaranteedCards.Clear();
            
            foreach (var card in cards)
            {
                availableCards.Add(card);
                
                if (card.dropGuaranteed)
                {
                    var limit = 0;
                    while (limit++ < card.dropWeight) guaranteedCards.Add(card);
                }
            }
        }

        
        
        /*===============================
         *  Dropping
         ==============================*/
        public void DropLoot(SwarmActorController controller)
        {
            if (!Environment.CardService.CardSpaceAvailable) return;

            var drop = Random.Range(0f, 1f); //drop value

            if (drop <= baseDropRate)
            {
                var card = GetCard(baseDropRate);
                if (card != null) DroppedCard(card); //drops loot
            }
        }

        public void ForceDrop(Card card)
        {
            if (!Environment.CardService.CardSpaceAvailable) return;
            DroppedCard(card);
        }
        
        Card GetCard(float dropRate)
        {
            Card card = null;
            
            if (dropRate >= 1f && guaranteedCards.Count > 0)
                card = guaranteedCards[Random.Range(0, guaranteedCards.Count - 1)];

            if (card == null && availableCards.Count > 0)
                card = availableCards[Random.Range(0, availableCards.Count - 1)];

            if (card == null)
            {
                return null;
            }
            
            availableCards = availableCards
                              .Where(c => c != card)
                              .ToList();
            guaranteedCards = guaranteedCards
                              .Where(c => c != card)
                              .ToList();

            return card;
        }
    }
}