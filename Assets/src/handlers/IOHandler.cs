using System.Collections.Generic;
using src.actors.controllers;
using src.actors.controllers.impl;
using src.card.model;
using src.config;
using src.services.impl;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.handlers
{
    public class IOHandler : MonoBehaviour
    {
        [Header("Player Preferences:   UI")] 
        [SerializeField] Color accentColor;

        [SerializeField] Color  selectionColor;
        [SerializeField] float  transitionTime;

        readonly Dictionary<RenderTexture, Camera> textureCameraMap 
            = new Dictionary<RenderTexture, Camera>();

        readonly RenderTexture[] renderTextures = new RenderTexture[2];
        
        public Preferences preferences;

        GameObject cursor;

        LootService   lootService;
        PlayerService playerService;

        /*===============================
         *  Properties
         ==============================*/
        public Color SelectionColor => selectionColor;

        /*===============================
         *  Unity Lifecycle
         ==============================*/
        void Awake()
        {
            //get services
            lootService   = Environment.LootService;
            playerService = Environment.PlayerService;

            preferences = new Preferences(
                accentColor,
                selectionColor,
                transitionTime);
            
            renderTextures[0] = Resources.Load(Environment.RESOURCE_SCREEN_ONE)
                as RenderTexture;
            renderTextures[1] = Resources.Load(Environment.RESOURCE_SCREEN_TWO)
                as RenderTexture;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
                lootService.ForceDrop(CardRepository.BeaconCard(true, 1));
            else if (Input.GetKeyDown(KeyCode.W))
                lootService.ForceDrop(CardRepository.EyeCard(true, 1));
            else if (Input.GetKeyDown(KeyCode.E))
                lootService.ForceDrop(CardRepository.ExplosiveCard(true, 1));
            else if (Input.GetKeyDown(KeyCode.R))
                lootService.ForceDrop(CardRepository.LureCard(true, 1));
        }

        /*===============================
         *  Handling
         ==============================*/
        public void HandleHit(RaycastHit hit)
        {
            var selected = hit.collider.gameObject;
            if (selected.name.Equals(Environment.OVERHEAD_UI)) HandleSelection(selected.transform.parent.gameObject);
            if (selected.CompareTag(Environment.TAG_FLOOR)) HandleFloor(hit);
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



        /*===============================
         *  Helper Methods
         ==============================*/
        public bool AttachCameraToTexture(Camera displayCamera)
        {
            foreach (var entry in textureCameraMap)
            {
                if (entry.Value == null)
                {
                    var texture = entry.Key;
                    textureCameraMap.Remove(texture);
                    
                    displayCamera.targetTexture          = texture;
                    displayCamera.forceIntoRenderTexture = true;
                    
                    textureCameraMap.Add(texture, displayCamera);
                    return true;
                }
            }

            return false;
        }

        public void DetachCamera(Camera displayCamera)
        {
            foreach (var entry in textureCameraMap)
            {
                var texture = entry.Key;
                if (entry.Value == displayCamera)
                {
                    displayCamera.targetTexture = null;
                    
                    textureCameraMap.Remove(texture);
                    textureCameraMap.Add(texture, null);
                    
                    Destroy(displayCamera);
                }
            }
        }
        
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