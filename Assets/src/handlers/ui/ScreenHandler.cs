using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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
            //TODO add raycast filter
            if (Physics.Raycast(viewRay, out var hit)) io.HandleHit(hit);
        }

        protected abstract void HandleHit(RaycastHit hit);
    }
}