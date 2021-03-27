using System.Collections.Generic;
using src.card.model;
using src.level;
using src.util;

namespace src.services.impl
{
    public class CardService : IService
    {
        /*************** Card Drops ***************/
        public delegate void SetCardsToDrop(Card[] cards);

        public event SetCardsToDrop CardDrops = delegate { };
        
        
        readonly HashSet<Card> activeCards
            = new HashSet<Card>();

        Card[] possibleCards;
        //key: int -> wave number
        //value: Wave -> possible cards to spawn 
        readonly Card[][] cardBatches= CardRepository.AllBatches;

        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.WaveService.NextWave
                += LoadNextCards;

            //cardBatches = CardRepository.GetAll.ToArray();
        }

        /*===============================
        *  Handling
        ==============================*/
        public void AddCard(Card card)
        {
            Environment.Log(GetType(), "adding card");
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