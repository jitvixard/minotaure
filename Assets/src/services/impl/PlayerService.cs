using System.Collections.Generic;
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

        BuilderController   builder;
        PawnActorController player;
        
        GameObject          heatZone;
        GameObject          prototypeHeatZone;
        
        int   scrap;

        float shotSpeed = Environment.COMBAT_PROJECTILE_SPEED;


        
        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            loadTime = Environment.COMBAT_LOAD_TIME;
            
            prototypeHeatZone =
                Resources.Load(Environment.RESOURCE_HEAT_ZONE)
                    as GameObject;

            cardService = Environment.CardService;
            
            //subscriptions
            Environment.BuilderService.Builder += BuilderAppeared;
            
            Environment.LootService.DroppedCard  += AddCard;
            Environment.LootService.DroppedScrap += AddScrap;
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
                Object.Destroy(heatZone);
            }

            player = controller;
            player.Select(true);
            heatZone = Object.Instantiate(
                prototypeHeatZone,
                player.transform);

            heatZone.name = "heat-zone" + heatZone.GetInstanceID();

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
    }
}