using System;
using src.actors.controllers;
using src.actors.controllers.impl;
using src.ai.swarm;
using src.config;
using src.config.control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Environment = src.util.Environment;

namespace src.io
{
    public class IOHandler : MonoBehaviour
    {
        new Camera camera;
        ControlConfig control; //config relating to control input

        public Preferences preferences;

        [Header("Player Preferences:   UI")] 
        [SerializeField] Color accentColor;
        [SerializeField] Color selectionColor;
        [SerializeField] float transitionTime;


        SwarmService swarmService;
        
        //Buffer for selected actors
        AbstractActorController selectedActor;
        PawnActorController selectedPawn;
        
        /*===============================
         *  Properties
         ==============================*/
        public Color SelectionColor => selectionColor;
        public AbstractActorController SelectedActor => selectedActor;
        public PawnActorController SelectedPawn => selectedPawn;


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

            swarmService = Environment.SwarmService;
            swarmService.IO = this;
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
            
            //TODO check to see if building

            if (controller is PawnActorController)
            {
                if (!(selectedPawn == null)) 
                    selectedPawn.Select(false); //deselect old
                selectedPawn = controller.Select(true) as PawnActorController; //select new
            }
            
            if (!(selectedActor == null)) selectedActor.Select(false); //deselect old
            selectedActor = controller.Select(true); //select new
        }
        
        void HandleFloor(Vector3 point)
        {
            if (selectedPawn) selectedPawn.Move(point);
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