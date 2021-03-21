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

        readonly GameObject prototypeHeatZone;
        
        PawnActorController player;
        GameObject heatZone;
        
        
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
        
        
        /*===============================
         *  Constructor
         ==============================*/
        public PlayerService()
        {
            prototypeHeatZone = 
                Resources.Load(Environment.RESOURCE_HEAT_ZONE) 
                    as GameObject;
        }
        
        
        /*===============================
         *  Handling
         ==============================*/
        public void Possess(PawnActorController controller)
        {
            if (player)
                player.Select(false);
            player = controller.Select(true) as PawnActorController;
        }

        public void ClickedFloor(Vector3 hitPoint)
        {
            if (player) player.Move(hitPoint);
        }
    }
}