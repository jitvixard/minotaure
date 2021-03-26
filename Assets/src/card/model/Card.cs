using System;
using src.model;
using UnityEngine;

namespace src.card.model
{
    [Serializable]
    public class Card
    {
        public readonly string title;
        public readonly string description;
        
        public readonly GameObject prototype;
        public readonly CardType   type;
        
        public readonly bool dropGuaranteed;
        public readonly int  dropWeight;

        public Card(
            CardType type,
            GameObject prototype,
            string title,
            string description)
        {
            this.type = type;
            this.prototype = prototype;
            this.title = title;
            this.description = description;
        }

        public static CardBuilder Builder => new CardBuilder();
    }

    public class CardBuilder
    {
        string   title;
        string   description;
        
        CardType type;

        public Card Build()
        {
            return new Card(
                type,
                GetPrototype(),
                title,
                description);
        }

        public Card BuildFrom(Card card)
        {
            return Card.Builder
                       .Build();
        }

        public CardBuilder Type(CardType type)
        {
            this.type = type;
            return this;
        }

        public CardBuilder Title(string title)
        {
            this.title = title;
            return this;
        }

        public CardBuilder Description(string description)
        {
            this.description = description;
            return this;
        }

        GameObject GetPrototype()
        {
            switch (type)
            {
                case CardType.Blank:
                    return null;
                case CardType.Tabular:
                    return null;
                default:
                    return null;
            }
        }
    }
}