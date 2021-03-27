using System;
using src.model;
using UnityEngine;
using Environment = src.util.Environment;

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

        bool guaranteedDrop;
        int  dropWeight;

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

        public CardBuilder GuaranteedDrop(bool guaranteedDrop)
        {
            this.guaranteedDrop = guaranteedDrop;
            return this;
        }

        public CardBuilder DropWeight(int dropWeight)
        {
            this.dropWeight = dropWeight;
            return this;
        }

        GameObject GetPrototype()
        {
            switch (type)
            {
                case CardType.Eye:
                    return Resources.Load(Environment.RESOURCE_CARD_EYE) 
                        as GameObject;
                case CardType.Tabular:
                    return null;
                default:
                    return null;
            }
        }
    }
}