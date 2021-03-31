using System;
using System.Collections.Generic;
using System.Linq;
using src.card.behaviours;
using src.card.behaviours.impl;
using src.handlers;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using Environment = src.util.Environment;

namespace src.buildings.controllers
{
    public class BeaconController : TowerController
    {
        List<GameObject> associatied = new List<GameObject>();
        
        IOHandler io;
        
        Camera beaconCamera;

        Color selectionColor = Color.white;
        
        GameObject cameraRig;
        GameObject explosive;

        ProceduralImage indicator;
        
        public Color SelectionColor
        {
            get => selectionColor;
            set
            {
                if (selectionColor == Color.white)
                {
                    selectionColor  = value;
                    indicator.color = selectionColor;
                }
            }
        }


        /*===============================
        *  Lifecycle
        ==============================*/
        protected override void Awake()
        {
            base.Awake();
            foreach (var child in GetComponentsInChildren<Transform>(true))
            {
                if (child.name == Environment.BEACON_CAMERA_RIG) cameraRig     = child.gameObject;
                else if (child.name == Environment.BEACON_EXPLOSIVE) explosive = child.gameObject;
            }

            io           = Camera.main.GetComponent<IOHandler>();
            beaconCamera = GetComponentInChildren<Camera>(true);

            indicator = GetComponentsInChildren<ProceduralImage>()
                .First(img => img.name.Contains(Environment.BEACON_INDICATOR));
        }

        /*===============================
        *  Handling
        ==============================*/ 
        public void Setup(CardBehaviour cardBehaviour)
        {
            if (cardBehaviour is EyeBehaviour eye)
                if (io.AttachCameraToTexture(beaconCamera))
                    eye.SetUpAsButton(this); //turn beacon into a camera
            if (cardBehaviour is ExplosiveBehaviour exp)
                if (exp.SetUpAsButton(this)) 
                    explosive.SetActive(true);
        }


        /*===============================
        *  IDestroyable
        ==============================*/
        public override float ExtraOffset() =>  Environment.BEACON_OFFSET;

        void OnDestroy()
        {
            foreach (var a in associatied)
            {
                Destroy(a);
            }
            
            builderService.TargetDestroyed(this);
        }
    }
}
