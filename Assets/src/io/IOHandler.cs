using System;
using src.config.control;
using src.player;
using UnityEngine;
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
            if (control.OnClick()) Cast();
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
    }
}