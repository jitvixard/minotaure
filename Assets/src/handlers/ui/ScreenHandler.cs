using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Environment = src.util.Environment;

namespace src.handlers.ui
{
    public abstract class ScreenHandler : MonoBehaviour, IPointerClickHandler
    {
        //cameras
        [SerializeField] Camera castingCamera;

        NavMeshAgent agent;

        //raycasting
        int filterLayer;

        IOHandler io;

        RectTransform screenTransform;

        void Awake()
        {
            io = Camera.main.GetComponent<IOHandler>();
            filterLayer = castingCamera.gameObject.layer;

            screenTransform = GetComponent<RectTransform>();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            var point = IOHandler.ScreenClickToViewportPoint(screenTransform, Camera.main);
            var viewRay = castingCamera.ViewportPointToRay(point);
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

        protected abstract void HandleHit(RaycastHit hit);
    }
}