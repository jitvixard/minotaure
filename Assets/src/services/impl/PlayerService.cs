using src.actors.controllers.impl;
using src.util;
using UnityEngine;

namespace src.services.impl
{
    public class PlayerService : IService
    {
        /*===============================
         *  Observable
         ==============================*/
        public delegate void CurrentPlayer(PawnActorController p);
        public event CurrentPlayer Player = delegate {  };
        
        /*===============================
         *  Fields
         ==============================*/
        PawnActorController player;
        
        GameObject heatZone;
        GameObject prototypeHeatZone;
        
        
        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
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
            {
                player.Select(false);
                GameObject.Destroy(heatZone);
            }
            
            player = controller;
            player.Select(true);
            heatZone = GameObject.Instantiate(
                prototypeHeatZone,
                player.transform);

            if (player) //truthy check to be safe
                Player(player); //emits event when a new player is selected
        }

        public void FloorClick(Vector3 hitPoint)
        {
            if (player) player.Move(hitPoint);
        }
    }
}