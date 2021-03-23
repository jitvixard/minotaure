using System.Collections;
using System.Linq;
using src.handlers;
using src.util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using static UnityEngine.Color;

namespace src.button
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        //Trigger
        protected EventTrigger trigger;
        //Colors
        Color accentColor;
        Color hoverColor;
        //Border & Text
        ProceduralImage border;
        Text text;
        //Routine
        Coroutine selectionRoutine;

        float transitionDuration;

        public bool ready = false; 
        bool selected = false;

        void Awake()
        {
            var preferences = Camera.main.GetComponent<IOHandler>().preferences;

            trigger = GetComponent<EventTrigger>();
            border = GetBorder();
            text = GetComponentInChildren<Text>();
            accentColor = preferences.accentColor;
            hoverColor = preferences.selectionColor;
            text.color = accentColor;
            border.color = accentColor;
            transitionDuration = preferences.transitionDuration;
        
            AddPointerDown();
            AddPointerEnter();
            AddPointerExit();
        }

        public abstract void OnPointerDownDelegate();
    
        public void OnPointerEnterDelegate()
        {
            if (!ready) return;
            selected = true;
            if (!(selectionRoutine is null)) StopCoroutine(selectionRoutine);
            selectionRoutine = StartCoroutine(SelectionRoutine());
        }
    
        public void OnPointerExitDelegate()
        {
            if (!ready) return;
            selected = false;
            if (!(selectionRoutine is null)) StopCoroutine(selectionRoutine);
            selectionRoutine = StartCoroutine(SelectionRoutine());
        }

        IEnumerator SelectionRoutine()
        {
            var targetColor = selected ? hoverColor : accentColor;
            var startColor = text.color;
            var duration = selected ? transitionDuration / 2 : transitionDuration;
            var t = 0f;

            while (!text.color.Compare(targetColor))
            {
                t += Time.deltaTime / duration;
                text.color = Lerp(startColor, targetColor, t);
                border.color = Lerp(startColor, targetColor, t);
                yield return null;
            }
        }

        void AddPointerDown()
        {
            //handles click
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
            entry.callback.AddListener((data) => { OnPointerDownDelegate(); });
            trigger.triggers.Add(entry);
        }
    
        void AddPointerEnter()
        {
            //handle mouse over
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
            entry.callback.AddListener((data) => { OnPointerEnterDelegate(); });
            trigger.triggers.Add(entry);
        }
    
        void AddPointerExit()
        {
            //handle mouse exit
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
            entry.callback.AddListener((data) => { OnPointerExitDelegate(); });
            trigger.triggers.Add(entry);
        }

        ProceduralImage GetBorder()
        {
            return GetComponentsInChildren<ProceduralImage>()
                .FirstOrDefault(image => 
                    image.name.Equals("border"));
        }
    }
}
