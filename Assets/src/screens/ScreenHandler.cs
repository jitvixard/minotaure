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

        Camera mainCamera;

        //raycasting
        int filterLayer;

        NavMeshAgent agent;

        RectTransform screenTransform;

        void Awake()
        {
            agent = GameObject.Find("actor").GetComponent<NavMeshAgent>();

            mainCamera = Camera.main;
            filterLayer = castingCamera.gameObject.layer;

            screenTransform = GetComponent<RectTransform>();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            print("boom");
            var point = IOHandler.ScreenClickToViewportPoint(screenTransform, Camera.main);
            var viewRay = castingCamera.ViewportPointToRay(point);
            if (Physics.Raycast(viewRay, out var hit))
            {
                print("hit " + hit.collider.name);
                agent.SetDestination(hit.point);
            }
        }

        protected abstract void HandleHit(RaycastHit hit);
    }
}
