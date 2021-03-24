using src.actors.controllers.impl;
using src.handlers;
using src.model;
using src.scripting.level;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class LootService : IService
    {
        public delegate void SetCard(Card card);
        public event SetCard DroppedCard = delegate {  };
        public delegate void SetScrap(int scrap);
        public event SetScrap DroppedScrap = delegate {  };
        
        
        Wave currentWave;

        Card card;
        int scrap;

        float baseDropRate = Environment.LOOT_DROP_RATE;
        float dynamicDropRate = 0f;

        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            Environment.WaveService.NextWave += ReadyLoot;
            Environment.SwarmService.Remaining += UpdateLoot;
        }
        
        /*===============================
         *  Loot Updates
         ==============================*/
        void ReadyLoot(Wave wave)
        {
            IOHandler.Log(GetType(), "Readying loot");
            currentWave = wave;
            QueueLoot();
        }

        void UpdateLoot(int remaining)
        {
            IOHandler.Log(GetType(), "Updating loot");
            dynamicDropRate = (float) currentWave.guaranteedDrops / remaining;
        }

        public void DropLoot(SwarmActorController controller)
        {
            IOHandler.Log(GetType(), "Dropping loot");
            var dropRate = baseDropRate > dynamicDropRate
                ? baseDropRate
                : dynamicDropRate;
            var drop = Random.Range(0f, 1f);
            if (drop <= dropRate) DropCard();
            DropScrap();
        }

        /*===============================
         *  Dropping
         ==============================*/
        void QueueLoot()
        {
            card = Card.BlankCard();
        }

        void DropCard()
        {
            DroppedCard(card);
        }
        
        void DropScrap()
        {
            DroppedScrap(100);
        }
    }
}