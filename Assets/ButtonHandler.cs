using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour
{
    EventTrigger trigger;
    
    void Awake()
    {
        trigger = GetComponent<EventTrigger>();
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
        print("pointer enter");
    }
    
    public void OnPointerExitDelegate(PointerEventData data)
    {
        print("pointer exit");
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
    
}
