using System;
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
        PlayerController playerController; //controls player character

        void Awake()
        {
            camera = Camera.main;
            control = ControlConfig.GetControl();
            playerController = GameObject.FindWithTag(Environment.PLAYER).GetComponent<PlayerController>();
        }

        void Update()
        {
            //if (control.OnClick()) Cast();
        }

        void Cast()
        {
            if (!Physics.Raycast(camera.ScreenPointToRay(control.InputPosition()), out var hit, 100f)) return;
            
            switch (hit.collider.gameObject.layer)
            {
                case Environment.LAYER_FLOOR:
                    playerController.Move(hit.collider.gameObject);
                    break;
            }
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
    }
}