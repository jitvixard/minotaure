using src.card.behaviours;
using src.card.model;
using src.util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardActionButton : MonoBehaviour, IPointerClickHandler
{
    CardBehaviour bufferedCard;

    TextMeshProUGUI text;


    /*===============================
    *  Lifecycle
    ==============================*/
    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        Environment.CardService.CardSelected += LoadCard;
    }

    
    /*===============================
    *  Handling
    ==============================*/
    public void LoadCard(Card card)
    {
        var cardBehaviour = card.behaviour;
        
        if (cardBehaviour is null)
        {
            Clear();
            return;
        }

        bufferedCard = cardBehaviour;
        text.text    = cardBehaviour.Card.title;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (bufferedCard is null) return;
        print("playing card " + bufferedCard.name);
    }
    
    
    /*===============================
    *  Util
    ==============================*/
    void Clear()
    {
        bufferedCard = null;
        text.text    = "";
    }
}
