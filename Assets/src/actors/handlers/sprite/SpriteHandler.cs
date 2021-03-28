using System.Collections;
using System.Linq;
using src.actors.controllers;
using src.handlers;
using src.util;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using static UnityEngine.Color;

namespace src.actors.handlers.sprite
{
    public class SpriteHandler
    {
        protected readonly AbstractActorController controller;
        protected readonly SpriteRenderer          sprite;
        
        protected readonly GameObject gameObject;

        protected readonly Transform parent;
        protected readonly float     rotationalOrigin;

        protected readonly Color original;
        protected readonly Color selected;

        protected Coroutine transitionRoutine;
        protected Coroutine rotationRoutine;

        public SpriteHandler(AbstractActorController controller)
        {
            sprite   = controller.GetComponentInChildren<SpriteRenderer>();
            original = sprite.color;
            selected = Camera.main.GetComponent<IOHandler>().SelectionColor;

            this.controller  = controller;
            parent           = controller.transform;
            rotationalOrigin = parent.transform.rotation.y;

            gameObject = controller.GetComponentsInChildren<Transform>()
                .First(t => t.name == Environment.OVERHEAD_UI)
                .gameObject;

            rotationRoutine = controller.StartCoroutine(LockUIRotation());
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

        IEnumerator LockUIRotation()
        {
            while (true)
            {
                var offset = parent.rotation.y - rotationalOrigin;
                gameObject.transform.rotation = Quaternion.Euler(90, 45 - offset, 0);
                yield return null;
            }
        }
    }
}