using System.Linq;
using src.card.behaviours;
using src.card.model;
using UnityEngine;
using Environment = src.util.Environment;

public class CardHandler : MonoBehaviour
{
	CardBehaviour[] cards;
	Transform[]     slots;

	GameObject prototypeSelectionLamp;
	GameObject selectionLamp;
	
	
	
	/*===============================
    *  Lifecycle
    ==============================*/
	void Awake()
	{
		slots = GetComponentsInChildren<Transform>()
			.Where(t => t.name.Contains(Environment.UI_POINT_NAMES))
			.ToArray();

		cards = new CardBehaviour[slots.Length];
		
		prototypeSelectionLamp = Resources.Load(Environment.RESOURCE_SELECTION_LIGHT) 
			as GameObject;

		Environment.CardService.CardSelected += SelectCard;
	}


	/*===============================
    *  Handling
    ==============================*/
	public void Add(Card card)
	{
		var i = 0;
		while (i < cards.Length)
		{
			if (cards[i] is null)
			{
				var cardObject = Instantiate(
					card.prototype,
					slots[i].position,
					slots[i].rotation);
				
				if (!cardObject.TryGetComponent<CardBehaviour>(out var behaviour)) break;

				behaviour.Card = card;
				card.behaviour = behaviour;

				cards[i] = behaviour;
				
				break;
			}
			i++;
		}
	}

	public void Remove(Card card)
	{
		var i = 0;
		while (i < cards.Length)
		{
			if (cards[i] == card.behaviour)
			{
				Destroy(card.behaviour.gameObject);
				cards[i] = null;
				break;
			}
		}
	}

	public void SelectCard(Card card)
	{
		CardBehaviour selectedCard = null;
		foreach (var behaviour in cards)
		{
			if (behaviour == card.behaviour)
			{
				selectedCard = behaviour;
				break;
			}
		}
		
		if (selectedCard is null) return;

		if (selectionLamp == null)
		{
			selectionLamp = Instantiate(
				prototypeSelectionLamp,
				selectedCard.transform.position,
				prototypeSelectionLamp.transform.rotation
			);

			selectionLamp.transform.parent = transform;
		}

		var hoverPosition = selectedCard.transform.position;
		selectionLamp.transform.position = new Vector3(
			hoverPosition.x,
			hoverPosition.y + 1f,
			hoverPosition.z);
	}
	
	
	
	/*===============================
    *  Utility
    ==============================*/
	public int MaxCards() => cards.Length;


}
