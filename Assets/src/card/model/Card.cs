using System;
using src.card.behaviours;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.card.model
{
    [Serializable]
    public class Card
    {
        public readonly string description;
        
        public readonly GameObject prototype;
        public readonly CardType   type;
        
        public readonly bool dropGuaranteed;
        public readonly int  dropWeight;

        public CardBehaviour behaviour;

        public Card(
            CardType type,
            GameObject prototype,
            string description,
            bool dropGuaranteed,
            int dropWeight)
        {
            this.type           = type;
            this.prototype      = prototype;
            this.description    = description;
            this.dropGuaranteed = dropGuaranteed;
            this.dropWeight     = dropWeight;
        }

        public static CardBuilder Builder => new CardBuilder();
    }

    public class CardBuilder
    {
        string   description;
        
        CardType type;

        bool dropGuaranteed;
        int  dropWeight;

        public Card Build()
        {
            return new Card(
                type,
                GetPrototype(),
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
                case CardType.Beacon:
                    return Resources.Load(Environment.RESOURCE_CARD_BEACON) 
                        as GameObject;
                case CardType.Explosive: 
                    return Resources.Load(Environment.RESOURCE_CARD_EXPLOSION)
                        as GameObject;
                case CardType.Eye:
                    return Resources.Load(Environment.RESOURCE_CARD_EYE) 
                        as GameObject;
                case CardType.Lure:
                    return Resources.Load(Environment.RESOURCE_CARD_LURE) 
                        as GameObject;
                default:
                    return null;
            }
        }
    }
}