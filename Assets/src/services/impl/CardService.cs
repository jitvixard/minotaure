using System.Collections.Generic;
using src.card.model;
using src.scripting.level;
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
        Card[][] cardBatches;

        /*===============================
        *  Initialization
        ==============================*/
        public void Init()
        {
            Environment.WaveService.NextWave
                += LoadNextCards;
            
            cardBatches = Environment.GetListFromJson<CardWrapper>().items;
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
                wave.number >= cardBatches.Length
                    ? GenerateCards(wave)
                    : cardBatches[wave.number];
            CardDrops(possibleCards);
        }

        /*===============================
        *  Utility
        ==============================*/
        Card[] GenerateCards(Wave wave)
        {
            return null;
        }
    }
}