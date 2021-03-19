using System.Collections;
using src.actors.controllers;
using src.io;
using src.util;
using TMPro;
using UnityEngine;
using static UnityEngine.Color;

namespace src.actors.handlers
{
    public class SpriteHandler
    {
        public SpriteRenderer Sprite { get; set; }

        AbstractActorController controller;
        
        Color original;
        Color selected;

        Coroutine transitionRoutine;
        
        public SpriteHandler(AbstractActorController controller)
        {
            Sprite = controller.GetComponentInChildren<SpriteRenderer>();
            this.controller = controller;
            original = Sprite.color;
            selected = Camera.main.GetComponent<IOHandler>().GetSelectionColor();
        }

        public void Refresh()
        {
            var targetColor = controller.IsSelected()
                ? selected
                : original;
            
            if (targetColor.Compare(Sprite.color)) return;
            if (!(transitionRoutine is null)) controller.StopCoroutine(transitionRoutine);

            transitionRoutine = controller.StartCoroutine(ChangeColor(targetColor));
        }

        IEnumerator ChangeColor(Color targetColor)
        {
            var startColor = Sprite.color;
            var duration = controller.IsSelected() 
                ? Environment.UI_OVERHEAD_SELECTION_INTERVAL / 2 
                : Environment.UI_OVERHEAD_SELECTION_INTERVAL;
            var t = 0f;

            while (!Sprite.color.Compare(targetColor))
            {
                t += Time.deltaTime / duration;
                Sprite.color = Lerp(startColor, targetColor, t);
                yield return null;
            }
        }
    }
}