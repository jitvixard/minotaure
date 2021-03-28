using System;
using src.card.behaviours;
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
        public readonly GameObject cursor;
        public readonly CardType   type;
        
        public readonly bool dropGuaranteed;
        public readonly int  dropWeight;

        public CardBehaviour behaviour;

        public Card(
            CardType type,
            GameObject prototype,
            GameObject cursor,
            string title,
            string description,
            bool dropGuaranteed,
            int dropWeight)
        {
            this.type           = type;
            this.prototype      = prototype;
            this.cursor         = cursor;
            this.title          = title;
            this.description    = description;
            this.dropGuaranteed = dropGuaranteed;
            this.dropWeight     = dropWeight;
        }

        public static CardBuilder Builder => new CardBuilder();
    }

    public class CardBuilder
    {
        string   title;
        string   description;
        
        CardType type;

        bool dropGuaranteed;
        int  dropWeight;

        public Card Build()
        {
            return new Card(
                type,
                GetPrototype(),
                GetCursor(),
                title,
                description,
                dropGuaranteed,
                dropWeight);
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
            this.dropGuaranteed = guaranteedDrop;
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
                case CardType.Beacon:
                    return Resources.Load(Environment.RESOURCE_CARD_BEACON) 
                        as GameObject;
                default:
                    return null;
            }
        }
        
        GameObject GetCursor()
        {
            switch (type)
            {
                case CardType.Eye:
                    return Resources.Load(Environment.RESOURCE_CURSOR_EYE) 
                        as GameObject;
                case CardType.Beacon:
                    return Resources.Load(Environment.RESOURCE_CURSOR_BEACON) 
                        as GameObject;
                default:
                    return null;
            }
        }
    }
}