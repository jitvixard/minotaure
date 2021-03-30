using System.Collections.Generic;
using System.Linq;
using src.actors.controllers;
using src.actors.controllers.impl;
using src.card.model;
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
        public delegate void UpdateLoot(List<Card> card, int scrap);
        public event CurrentPlayer Player      = delegate { };
        public event UpdateLoot    LootChanged = delegate { };


        
        /*===============================
         *  Fields
         ==============================*/
        public PawnActorController PlayerPawn      => player;
        public float               ProjectileSpeed => shotSpeed;
        public int                 Scrap           => scrap;
        
        
        /*===============================
         *  Fields
         ==============================*/
        readonly List<Card> cards = new List<Card>();

        public float loadTime;

        CardService cardService;

        GameObject playerPrototype;

        BuilderController   builder;
        PawnActorController player;

        int   scrap;

        float shotSpeed = Environment.COMBAT_PROJECTILE_SPEED;


        
        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            loadTime    = Environment.COMBAT_LOAD_TIME;
            cardService = Environment.CardService;
            
            //subscriptions
            Environment.BuilderService.Builder  += BuilderAppeared;
            Environment.LootService.DroppedCard += AddCard;
            
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

        void AddCard(Card card)
        {
            cards.Add(card);
            cardService.AddCard(card);
            LootChanged(cards, scrap);
        }

        void AddScrap(int scrap)
        {
            this.scrap += scrap;
            LootChanged(cards, this.scrap);
        }

        void BuilderAppeared(BuilderController builder)
        {
            this.builder =  builder;
            scrap        -= Environment.BUILD_COST;
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