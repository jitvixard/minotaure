using System.Collections;
using System.Collections.Generic;
using System.Linq;
using src.card.behaviours;
using src.card.model;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using Environment = src.util.Environment;

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
        TextMeshProUGUI buttonText;
        ProceduralImage joiner;
        ProceduralImage button;
        
        
        
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

            var parent = transform.parent;
            joiner = parent.GetComponentsInChildren<ProceduralImage>()
                .First(p => p.name == Environment.UI_CARD_JOINER);
            button = parent.GetComponentsInChildren<ProceduralImage>()
                .First(p => p.name == Environment.UI_CARD_BUTTON);
            buttonText = parent.GetComponentsInChildren<TextMeshProUGUI>()
                .First(p => p.name == Environment.UI_CARD_BUTTON_TEXT);
        }

        

        /*===============================
        *  UI
        ==============================*/
        protected override IEnumerator TransitionRoutine()
        {
            var start = rectTransform.position;
            var target = isOut ? origin : displayed;
            var duration = Environment.UI_CARD_SLIDE_OUT;
            var t = 0f;

            isOut = !isOut;

            while (t < duration)
            {
                rectTransform.position = new Vector3(
                    Mathf.Lerp(start.x, target.x, t / duration),
                    Mathf.Lerp(start.y, target.y, t / duration),
                    Mathf.Lerp(start.z, target.z, t / duration));
                t += Time.deltaTime;
                yield return null;
            }


            rectTransform.position = target;
            
            var alphaOrigin = joiner.color.a;
            var goal = alphaOrigin >= 0.99f ? 0f : 1f;
            var waitTime = Environment.UI_BUTTON_FADE;

            t = 0f;

            do
            {
                t += Time.deltaTime / waitTime;
                var tempColor = joiner.color;
                tempColor.a = Mathf.Lerp(alphaOrigin, goal, t);

                joiner.color = tempColor;
                yield return null;
            } while (joiner.color.a != goal);

            t = 0f;
            do
            {
                t += Time.deltaTime / waitTime;
                
                var tempColor = button.color;
                tempColor.a = Mathf.Lerp(alphaOrigin, goal, t);

                var tempTextColor = buttonText.color;
                tempTextColor.a = Mathf.Lerp(alphaOrigin, goal, t);

                buttonText.color = tempTextColor;
                button.color     = tempColor;
                yield return null;
            } while (button.color.a != goal 
            && buttonText.color.a != goal);
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