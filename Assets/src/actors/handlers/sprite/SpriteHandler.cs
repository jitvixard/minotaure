using System.Collections;
using System.Linq;
using src.actors.controllers;
using src.actors.controllers.impl;
using src.handlers;
using src.util;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

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
        protected Coroutine slashRoutine;

        protected ProceduralImage[] possessionIndicators;

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

            possessionIndicators = gameObject.GetComponentsInChildren<ProceduralImage>()
                .Where(img => img.name.Contains(Environment.PAWN_POSSESSION_INDICATOR))
                .ToArray();
        }

        public void Possess()
        {
            if (!(transitionRoutine is null)) controller.StopCoroutine(transitionRoutine);
            transitionRoutine = controller.StartCoroutine(PossessionRoutine());
        }

        public void Slash(PawnActorController target)
        {
            if (slashRoutine != null) controller.StopCoroutine(slashRoutine);
            slashRoutine = controller.StartCoroutine(SlashRoutine(target));
        }

        IEnumerator PossessionRoutine()
        {
            var targetA = controller.IsSelected
                ? 1f
                : 0f;
            
            var startA = possessionIndicators[0].color.a;
            var duration = controller.IsSelected
                ? Environment.UI_OVERHEAD_SELECTION_INTERVAL / 2
                : Environment.UI_OVERHEAD_SELECTION_INTERVAL;
            var t = 0f;

            while (t < duration)
            {
                var tempColor = possessionIndicators[0].color;
                tempColor.a = Mathf.Lerp(startA, targetA, t / duration);

                foreach (var img in possessionIndicators)
                    img.color = tempColor;
                
                t += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator SlashRoutine(PawnActorController target)
        {
            var origin = gameObject.transform.localPosition;
            var targetDistance = origin.z + Environment.SWARM_ATTACK_JAB_DISTANCE;
            var duration = Environment.SWARM_ATTACK_SPEED;

            var t = 0f;
            while (t < duration)
            {
                var tempVector = new Vector3(
                    origin.x,
                    origin.y,
                    Mathf.Lerp(origin.z, targetDistance, t / duration));
                gameObject.transform.localPosition =  tempVector;
                
                t += Time.deltaTime;
                yield return null;
            }
            
            if (controller is SwarmActorController sac) target.TakeDamage(sac);
            
            origin         =  gameObject.transform.localPosition;
            targetDistance =  0;
            duration       /= 3;

            t = 0f;
            while (t < duration)
            {
                var tempVector = new Vector3(
                    origin.x,
                    origin.y,
                    Mathf.Lerp(origin.z, targetDistance, t / duration));
                gameObject.transform.localPosition = tempVector;
                
                t += Time.deltaTime;
                yield return null;
            }
            
            
        }
    }
}