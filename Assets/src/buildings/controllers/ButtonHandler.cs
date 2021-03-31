using System.Linq;
using src.button;
using src.card.model;
using src.util;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    readonly ButtonBehaviour[] buttons = new ButtonBehaviour[3];
	Transform[] slots = new Transform[3];
    	
    	
    	
    	/*===============================
        *  Lifecycle
        ==============================*/
    	void Awake()
    	{
    		slots = GetComponentsInChildren<Transform>()
    			.Where(t => t.name.Contains(Environment.UI_POINT_NAMES))
    			.ToArray();
    	}
    
    	
    	
    	/*===============================
        *  Handling
        ==============================*/
    	public void Add(ButtonBehaviour button)
    	{

    	}
    
    	public void Remove(Card card)
    	{

    	}
}
