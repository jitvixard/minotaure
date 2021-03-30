using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace src.handlers.ui
{
    public class OverheadScreenHandler : ScreenHandler
    {
        protected override void HandleRay(Ray viewRay, PointerEventData eventData)
        {
            if (Physics.Raycast(viewRay, out var hit)) 
                switch (eventData.button)
                {
                    case PointerEventData.InputButton.Left:
                        io.HandleHit(hit);
                        break;
                    case PointerEventData.InputButton.Right:
                        io.HandleAction(hit);
                        break;
                    case PointerEventData.InputButton.Middle:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }
    }
}