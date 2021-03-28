using System.Collections.Generic;
using System.Linq;
using src.actors.controllers.impl;
using src.card.model;
using src.level;
using src.scripting.level;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class LootService : IService
    {
        /*************** Card Observable ***************/
        public delegate void SetCard(Card card);
        public event SetCard DroppedCard = delegate { };
        /*************** Scrap Observable ***************/
        public delegate void SetScrap(int scrap);
        public event SetScrap DroppedScrap = delegate { };
        
        
        /*===============================
        *  Fields
        ==============================*/
        List<Card> availableCards  = new List<Card>();
        List<Card> guaranteedCards = new List<Card>();
        int guaranteedDrops;

        readonly float baseDropRate = Environment.LOOT_DROP_RATE;


        float dynamicDropRate;
        int   scrap;

        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            Environment.WaveService.NextWave += ReadyLoot;
            Environment.SwarmService.Remaining += UpdateLoot;
            Environment.CardService.CardDrops += QueueLoot;
        }

        /*===============================
         *  Loot Updates
         ==============================*/
        void ReadyLoot(Wave wave)
        {
            //TODO update scrap????
        }

        void UpdateLoot(int remaining)
        {
            dynamicDropRate = (float) guaranteedDrops / remaining;
        }

        /*===============================
         *  Preparation
         ==============================*/
        void QueueLoot(Card[] cards)
        {
            availableCards.Clear();
            guaranteedCards.Clear();
            guaranteedDrops = 0;
            
            foreach (var card in cards)
            {
                availableCards.Add(card);
                
                if (card.dropGuaranteed)
                {
                    guaranteedDrops++;
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
            var dropRate = baseDropRate > dynamicDropRate
                ? baseDropRate
                : dynamicDropRate;
            var drop = Random.Range(0f, 1f); //drop value

            if (drop <= dropRate)
            {
                var card = GetCard(dropRate);
                if (card != null) DroppedCard(card); //drops loot
            }
            
            DroppedScrap(GetScrap()); //drops scrap
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

        int GetScrap()
        {
            return 100;
        }

        void DropScrap(int amount) => DroppedScrap(amount);
    }
}