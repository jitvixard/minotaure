using src.actors.controllers.impl;
using src.io;
using src.util;
using UnityEngine;

namespace src.player
{
    public class PlayerService
    {
        /*===============================
         *  Fields
         ==============================*/

        PawnActorController player;
        GameObject heatZone;

        GameObject prototypeHeatZone;


        /*===============================
         *  Properties
         ==============================*/
        public PawnActorController Player
        {
            get
            {
                if (player) return player;
                Debug.LogWarning("[PlayerService] Attempted Access of 'Player' whilst undefined");
                return null;
            }
            set => player = value;
        }
        public GameObject PrototypeHeatZone
        {
            get
            {
                if (prototypeHeatZone) prototypeHeatZone = 
                    Resources.Load(Environment.RESOURCE_HEAT_ZONE) 
                        as GameObject;
                return prototypeHeatZone;
            }
        }


        /*===============================
         *  Handling
         ==============================*/
        public void Possess(PawnActorController controller)
        {
            if (player)
            {
                player.Select(false);
                GameObject.Destroy(heatZone);
            }
            else
            {
                player = controller;
                player.Select(true);
                heatZone = GameObject.Instantiate(
                    PrototypeHeatZone,
                    player.transform);
            }
        }

        public void ClickedFloor(Vector3 hitPoint)
        {
            if (player) player.Move(hitPoint);
        }
    }
}