using System.Collections;
using System.Linq;
using src.actors.controllers;
using src.handlers;
using src.services.impl;
using src.util;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using static UnityEngine.Color;

namespace src.actors.handlers
{
    public class SpriteHandler
    {
        readonly AbstractActorController controller;
        readonly SpriteRenderer          sprite;

        readonly Transform parent;
        readonly float rotationalOrigin;

        readonly Color original;
        readonly Color selected;

        Coroutine transitionRoutine;
        Coroutine loadRoutine;
        Coroutine rotationRoutine;

        GameObject gameObject;

        ProceduralImage loadingIndicator;

        public SpriteHandler(AbstractActorController controller)
        {
            sprite   = controller.GetComponentInChildren<SpriteRenderer>();
            original = sprite.color;
            selected = Camera.main.GetComponent<IOHandler>().SelectionColor;

            loadingIndicator            = controller.GetComponentInChildren<ProceduralImage>();
            loadingIndicator.fillAmount = 0f;
            
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

        public void Load()
        {
            if (loadRoutine != null) controller.StopCoroutine(loadRoutine);
            loadRoutine = controller.StartCoroutine(LoadRoutine());
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

        IEnumerator LoadRoutine()
        {
            var loadTime = Environment.PlayerService.loadTime;
            var origin = loadingIndicator.fillAmount;
            var targetValue = origin > 0.1f
                ? 0f
                : 1f;
            var t = 0f;
            
            while (loadingIndicator.fillAmount != targetValue)
            {
                t += Time.deltaTime;
                loadingIndicator.fillAmount = 
                    Mathf.Lerp(origin, targetValue, t / loadTime);
                
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