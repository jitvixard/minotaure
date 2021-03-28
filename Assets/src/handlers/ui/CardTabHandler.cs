using System.Collections.Generic;
using System.Linq;
using src.card.behaviours;
using src.card.model;
using src.services.impl;
using src.util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace src.handlers.ui
{
    public class CardTabHandler : TabHandler
    {
        /*===============================
        *  Fields
        ==============================*/
        
        /*=======Card Management======*/
        List<RectTransform> cardPositions = new List<RectTransform>();
        GameObject[]        cardTiles;
        Card[]              cards;
        
        /*===========Display==========*/
        TextMeshProUGUI cardText; 
        
        
        
        /*===============================
        *  Lifecycle
        ==============================*/
        protected override void Awake()
        {
            base.Awake();
            cardPositions = GameObject
                            .FindGameObjectsWithTag(Environment.TAG_CARD_PLACE_HOLDER)
                            .Select(g => g.GetComponent<RectTransform>())
                            .Where(rt => rt != null)
                            .ToList();
            cardTiles = new GameObject[cardPositions.Count];
            cards = new Card[cardPositions.Count];

            Environment.CardService.CardSelected += Focus;

            cardText = tab
                .GetComponentsInChildren<TextMeshProUGUI>()
                .First(t => t.name == Environment.UI_CARD_TEXT);
        }
        
        /*===============================
        *  Card Management
        ==============================*/
        void Focus(Card card)
        {
            cardText.text = card.description;
        }
        
        public bool AddCard(Card card)
        {
            var prototype = card.prototype;

            if (prototype is null)
            {
                Debug.LogWarning("Card '" + card.type + "' has no prototype.");
                return false;
            }

            var i = 0;
            while (i < cardPositions.Count)
                if (cards[i] == null) break;
                else i++;
        
            if (i == cardPositions.Count) return false;

            prototype = Instantiate(prototype, cardPositions[i]);
            prototype.name = "card" + prototype.GetInstanceID();
            prototype.transform.localPosition = Vector3.zero;

            var behaviour = prototype.GetComponent<CardBehaviour>();
            behaviour.Card = card;
                
            cardTiles[i] = prototype;
            cards[i] = card;

            return true;
        }

        public bool RemoveCard()
        {
            print("removing");
            var i = 0;
            while (i < cardPositions.Count)
                if (cards[i] != null) break;
                else i++;
        
            if (i == cardPositions.Count) return false;

            var tile = cardTiles[i];
            cardTiles[i] = null;
            cards[i] = null;
            Destroy(tile);
            return true;
        }
        
    }
}