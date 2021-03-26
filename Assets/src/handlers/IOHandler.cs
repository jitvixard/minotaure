using src.actors.controllers;
using src.actors.controllers.impl;
using src.config;
using src.config.control;
using src.services.impl;
using src.util;
using UnityEngine;

namespace src.handlers
{
    public class IOHandler : MonoBehaviour
    {
        [Header("Player Preferences:   UI")] [SerializeField]
        Color accentColor;

        [SerializeField] Color  selectionColor;
        [SerializeField] float  transitionTime;
        new              Camera camera;
        ControlConfig           control; //config relating to control input


        PlayerService playerService;

        public Preferences preferences;

        //Buffer for selected actors

        /*===============================
         *  Properties
         ==============================*/
        public Color                   SelectionColor => selectionColor;
        public AbstractActorController SelectedActor  { get; set; }

        /*===============================
         *  Unity Lifecycle
         ==============================*/
        void Awake()
        {
            camera = Camera.main;
            control = ControlConfig.GetControl();

            preferences = new Preferences(
                accentColor,
                selectionColor,
                transitionTime);

            //get services
            playerService = Environment.PlayerService;
        }

        /*===============================
         *  Handling
         ==============================*/
        public void HandleHit(RaycastHit hit)
        {
            var selected = hit.collider.gameObject;
            if (selected.name.Equals(Environment.OVERHEAD_UI)) HandleSelection(selected.transform.parent.gameObject);
            if (selected.CompareTag(Environment.TAG_FLOOR)) HandleFloor(hit.point);
            //TODO Handle Attack Case
            //TODO Handle PickUp Case
        }

        void HandleSelection(GameObject selected)
        {
            if (!selected.TryGetComponent<AbstractActorController>(out var controller)) return;

            if (controller is PawnActorController pac) playerService.Possess(pac);
            else if (controller is SwarmActorController sac) sac.Die();

            if (!(SelectedActor == null)) SelectedActor.Select(false); //deselect old
            SelectedActor = controller.Select(true);                   //select new
        }

        void HandleFloor(Vector3 point)
        {
            playerService.FloorClick(point);
        }

        /*===============================
         *  Helper Methods
         ==============================*/
        public static Vector3 ScreenClickToViewportPoint(RectTransform screenTransform, Camera inputCamera)
        {
            //TODO investigate loss of fractions 
            var anchoredPos = screenTransform.anchoredPosition;
            var xOffset = -(inputCamera.pixelWidth / 2 + anchoredPos.x);
            var yOffset = -(inputCamera.pixelHeight / 2 + anchoredPos.y);
            var xScreenHit = Input.mousePosition.x + xOffset;
            var yScreenHit = Input.mousePosition.y + yOffset;

            var screenHit = new Vector3(xScreenHit, yScreenHit, 0f);
            var rect = screenTransform.rect;
            return new Vector3(
                (screenHit.x + rect.width / 2) / rect.width,
                (screenHit.y + rect.height / 2) / rect.height,
                0);
        }
    }
}