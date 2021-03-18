using src.io;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace src.screens
{
    public abstract class ScreenHandler : MonoBehaviour, IPointerClickHandler
    {
        //cameras
        [SerializeField] Camera castingCamera;

        IOHandler io;

        //raycasting
        int filterLayer;

        NavMeshAgent agent;

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
