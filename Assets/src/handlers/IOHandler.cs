using src.actors.controllers;
using src.actors.controllers.impl;
using src.card.model;
using src.config;
using src.services.impl;
using src.util;
using UnityEngine;

namespace src.handlers
{
    public class IOHandler : MonoBehaviour
    {
        [Header("Player Preferences:   UI")] 
        [SerializeField] Color accentColor;

        [SerializeField] Color  selectionColor;
        [SerializeField] float  transitionTime;
        
        public Preferences preferences;

        GameObject cursorBase;
        GameObject cursor;

        CardService   cardService;
        PlayerService playerService;

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
            //get services
            cardService   = Environment.CardService;
            playerService = Environment.PlayerService;

            cursorBase = GameObject.FindGameObjectWithTag(Environment.TAG_CURSOR_BASE);

            preferences = new Preferences(
                accentColor,
                selectionColor,
                transitionTime);

            //subscriptions
            Environment.CardService.CardSelected += QueueCard;
        }

        /*===============================
         *  Handling
         ==============================*/
        public void HandleHit(RaycastHit hit)
        {
            var selected = hit.collider.gameObject;
            if (selected.name.Equals(Environment.OVERHEAD_UI)) HandleSelection(selected.transform.parent.gameObject);
            if (selected.CompareTag(Environment.TAG_FLOOR)) HandleFloor(hit);
            //TODO Handle Attack Case
            //TODO Handle PickUp Case
        }

        public void HandleAction(RaycastHit hit) => playerService.Action(hit);

        void HandleSelection(GameObject selected)
        {
            if (!selected.TryGetComponent<AbstractActorController>(out var controller)) return;
            if (controller is PawnActorController pac) playerService.Possess(pac);
        }

        void HandleFloor(RaycastHit hit)
        {
            playerService.FloorClick(hit);
        }

        void QueueCard(Card card)
        {
            ApplyCursor(card);
        }

        public GameObject DetachCursor()
        {
            var cursorReturn = cursor;
            cursor = null;
            return cursorReturn;
        }
        
        void ApplyCursor(Card card)
        {
            if (card == null)
            {
                Cursor.visible = true;
                if (cursor != null) Destroy(cursor);
                return;
            }

            Cursor.visible            = false;
            cursor                    = Instantiate(card.cursor, cursorBase.transform);
            cursor.transform.position = Input.mousePosition;
        }

        /*===============================
         *  Helper Methods
         ==============================*/
        public static Vector3 ScreenClickToViewportPoint(RectTransform screenTransform, Camera inputCamera)
        {
            var anchoredPos = screenTransform.anchoredPosition;
            var xOffset = -(inputCamera.pixelWidth / 2f + anchoredPos.x);
            var yOffset = -(inputCamera.pixelHeight / 2f + anchoredPos.y);
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