using src.card.behaviours;
using src.handlers.ui;
using src.util;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableHandler : ScreenHandler
{
    protected override void HandleRay(Ray viewRay, PointerEventData eventData)
    {
        if (Physics.Raycast(viewRay, out var hit))
        {
            if (hit.collider.gameObject.TryGetComponent<CardBehaviour>(out var card))
            {
                Environment.CardService.Focus(card.Card);
            }
        }
    }
}
