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
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(control.InputPosition()), out hit, 100f))
            {
                print("hit");
                switch (hit.collider.gameObject.layer)
                {
                    case Environment.LAYER_FLOOR:
                        print("moving");
                        playerController.MoveTo(hit.collider.gameObject);
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }
}