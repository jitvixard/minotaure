using System.Collections;
using src.actors.controllers;
using src.util;
using TMPro;
using UnityEngine;
using static UnityEngine.Color;

namespace src.actors.handlers
{
    public class SpriteHandler
    {
        readonly SpriteRenderer sprite;

        readonly AbstractActorController controller;
        
        Color original;
        Color selected;

        Coroutine transitionRoutine;
        
        public SpriteHandler(AbstractActorController controller)
        {
            sprite = controller.GetComponentInChildren<SpriteRenderer>();
            this.controller = controller;
            original = sprite.color;
            selected = Camera.main.GetComponent<IOHandler>().SelectionColor;
        }

        public void Refresh()
        {
            var targetColor = controller.IsSelected
                ? selected
                : original;
            
            if (targetColor.Compare(sprite.color)) return;
            if (!(transitionRoutine is null)) controller.StopCoroutine(transitionRoutine);

            transitionRoutine = controller.StartCoroutine(ChangeColor(targetColor));
        }

        IEnumerator ChangeColor(Color targetColor)
        {
            var startColor = sprite.color;
            var duration = controller.IsSelected 
                ? Environment.UI_OVERHEAD_SELECTION_INTERVAL / 2 
                : Environment.UI_OVERHEAD_SELECTION_INTERVAL;
            var t = 0f;

            while (!sprite.color.Compare(targetColor))
            {
                t += Time.deltaTime / duration;
                sprite.color = Lerp(startColor, targetColor, t);
                yield return null;
            }
        }
    }
}