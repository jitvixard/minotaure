using System.Linq;
using src.buildings.controllers;
using src.card.model;
using src.services.impl;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using Environment = src.util.Environment;

namespace src.card.behaviours
{
    public abstract class CardBehaviour : MonoBehaviour
    {
        public Card Card
        {
            set
            {
                if (card is null)
                {
                    value.behaviour = this;
                    card           = value;
                }
            }
            get => card;
        }

        protected CardService cardService;

        protected BeaconController attachedBeacon;
        
        protected Card      card;
        protected Coroutine routine;

        protected RaycastHit hit;

        protected bool asButton;

        /**************** Card Behaviour ****************/
        protected abstract bool BehaviourDirective(RaycastHit hit);

        public abstract bool SetUpAsButton(BeaconController reference);
        
        public abstract bool ExecuteAction();

        public bool IsButton() => asButton;

        /*===============================
        *  Lifecycle
        ==============================*/
        protected  virtual void Awake()
        {
            cardService = Environment.CardService;
        }

        public bool Play(RaycastHit hit)
        {
            this.hit = hit;
            return BehaviourDirective(hit);
        }



        /*===============================
        *  Setup
        ==============================*/
        protected void ColorSetup(BeaconController reference)
        {
            attachedBeacon = reference;
            foreach (var img in GetComponentsInChildren<ProceduralImage>(true))
            {
                if (img.name.Contains(Environment.BEACON_INDICATOR))
                {
                    img.enabled = true;
                    img.color   = reference.SelectionColor;
                    break;
                }
            }
        }
    }
}