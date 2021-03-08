using System;
using src.config;
using src.config.control;
using src.player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Environment = src.util.Environment;

namespace src.io
{
    public class IOHandler : MonoBehaviour
    {
        Camera camera;
        ControlConfig control; //config relating to control input

        public Preferences preferences;

        [Header("Player Preferences: UI")] 
        [SerializeField] Color accentColor;
        [SerializeField] Color selectionColor;
        [SerializeField] float transitionTime;

        void Awake()
        {
            camera = Camera.main;
            control = ControlConfig.GetControl();

            preferences = new Preferences(
                accentColor,
                selectionColor, 
                transitionTime);
        }

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

        public Color GetSelectionColor()
        {
            return preferences.selectionColor;
        }
    }
}