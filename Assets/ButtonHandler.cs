using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using static UnityEngine.Color;

public class ButtonHandler : MonoBehaviour
{
    //Trigger
    protected EventTrigger trigger;
    //Colors
    Color originalColor;
    [SerializeField] Color hoverColor;
    //Border & Text
    ProceduralImage border;
    Text text;
    //Routine
    Coroutine selectionRoutine;

    [SerializeField] float transitionDuration = 1f;
    bool selected = false;

    void Awake()
    {
        trigger = GetComponent<EventTrigger>();
        border = GetBorder();
        text = GetComponentInChildren<Text>();
        originalColor = text.color;
        
        AddPointerDown();
        AddPointerEnter();
        AddPointerExit();
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        print("pointer down");
    }
    
    public void OnPointerEnterDelegate(PointerEventData data)
    {
        selected = true;
        if (!(selectionRoutine is null)) StopCoroutine(selectionRoutine);
        selectionRoutine = StartCoroutine(SelectionRoutine());
        print("pointer exit");
    }
    
    public void OnPointerExitDelegate(PointerEventData data)
    {
        selected = false;
        if (!(selectionRoutine is null)) StopCoroutine(selectionRoutine);
        selectionRoutine = StartCoroutine(SelectionRoutine());
        print("pointer exit");
    }

    IEnumerator SelectionRoutine()
    {
        print("starting");
        var targetColor = selected ? hoverColor : originalColor;
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
        print("finishing");
    }

    void AddPointerDown()
    {
        //handles click
        var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    
    void AddPointerEnter()
    {
        //handle mouse over
        var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }
    
    void AddPointerExit()
    {
        //handle mouse exit
        var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
        entry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    ProceduralImage GetBorder()
    {
        return GetComponentsInChildren<ProceduralImage>()
            .FirstOrDefault(image => 
                image.name.Equals("border"));
    }
    
    
    
}
