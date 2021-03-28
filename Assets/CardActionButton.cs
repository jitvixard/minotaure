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
        if (card?.behaviour is null)
        {
            Clear();
            return;
        }

        bufferedCard = card.behaviour;
        text.text    = card.title;
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
