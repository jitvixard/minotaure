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

        //Loot
        readonly List<Card> cards = new List<Card>();

        GameObject heatZone;

        /*===============================
         *  Fields
         ==============================*/
        PawnActorController player;
        GameObject          prototypeHeatZone;
        int                 scrap;


        /*===============================
         *  Initialization
         ==============================*/
        public void Init()
        {
            prototypeHeatZone =
                Resources.Load(Environment.RESOURCE_HEAT_ZONE)
                    as GameObject;

            //subscriptions
            Environment.LootService.DroppedCard += AddCard;
            Environment.LootService.DroppedScrap += AddScrap;
        }

        public event CurrentPlayer Player      = delegate { };
        public event UpdateLoot    LootChanged = delegate { };


        /*===============================
         *  Handling
         ==============================*/
        public void Possess(PawnActorController controller)
        {
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

            if (player)         //truthy check to be safe
                Player(player); //emits event when a new player is selected
        }

        public void FloorClick(Vector3 hitPoint)
        {
            if (player) player.Move(hitPoint);
        }

        void AddCard(Card card)
        {
            cards.Add(card);
            LootChanged(cards, scrap);
        }

        void AddScrap(int scrap)
        {
            this.scrap += scrap;
            LootChanged(cards, this.scrap);
        }
    }
}