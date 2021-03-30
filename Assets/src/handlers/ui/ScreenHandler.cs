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
        [SerializeField] protected Camera castingCamera;
        
        protected IOHandler     io;
        protected RectTransform screenTransform;

        void Awake()
        {
            io              = Camera.main.GetComponent<IOHandler>();
            screenTransform = GetComponent<RectTransform>();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            var point = IOHandler.ScreenClickToViewportPoint(screenTransform, Camera.main);
            var viewRay = castingCamera.ViewportPointToRay(point);
            
            HandleRay(viewRay, eventData);
        }

        protected abstract void HandleRay(Ray viewRay, PointerEventData eventData);
    }
}