using System.Linq;
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
        public event CurrentPlayer Player      = delegate { };



        
        /*===============================
         *  Fields
         ==============================*/
        public float ProjectileSpeed => shotSpeed;


        /*===============================
         *  Fields
         ==============================*/
        public float loadTime;

        CardService cardService;

        GameObject          playerPrototype;
        
        PawnActorController player;

        float shotSpeed = Environment.COMBAT_PROJECTILE_SPEED;


        
        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            loadTime    = Environment.COMBAT_LOAD_TIME;
            cardService = Environment.CardService;

            playerPrototype = Resources.Load(Environment.RESOURCE_PAWN)
                as GameObject;
        }

        

        /*===============================
         *  Handling
         ==============================*/
        public void Possess(PawnActorController controller)
        {
            if (controller == player) return; //self ref (stop)
            
            if (player)
            {
                player.Select(false);
            }

            player = controller;
            player.Select(true);

            if (player)         //truthy check to be safe
                Player(player); //emits event when a new player is selected

            player.agent.isStopped = true;
        }

        public void Action(RaycastHit hit)
        {
            if (cardService.IsCardSelected) cardService.ActivateCard(hit);
            else if (player) player.Fire(hit);
        }

        public void FloorClick(RaycastHit hit)
        {
            if (player) player.Move(hit.point);
        }

        public void OnPlayerDeath(PawnActorController pac)
        {
            if (player == pac) Player(null);

            var spawnPoints = GameObject
                .FindGameObjectsWithTag(Environment.TAG_SEED)
                .Select(x => x.transform)
                .ToArray();
            
            if (spawnPoints.Length == 0) Environment.GameService.GameOver();

            var closest = spawnPoints[0];
            foreach (var point in spawnPoints)
            {
                if (Vector3.Distance(closest.position, pac.transform.position)
                    > Vector3.Distance(point.position, pac.transform.position))
                {
                    closest = point;
                }
            }
            
            Respawn(closest.gameObject);
        }

        void Respawn(GameObject spawnPoint)
        {
            var pawn = Object.Instantiate(
                playerPrototype,
                spawnPoint.transform.position,
                new Quaternion());

            if (!pawn.TryGetComponent<PawnActorController>(out var pac))
            {
                Environment.GameService.GameOver();
                return;
            }
            
            Possess(pac);
            Object.Destroy(spawnPoint);
        }
    }
}