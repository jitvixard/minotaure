﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using src.model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using Environment = src.util.Environment;

public class TabHandler : MonoBehaviour, IPointerClickHandler
{
    /*===============================
    *  Properties
    ==============================*/
    
    /*===============================
    *  Fields
    ==============================*/
    /*=======Card Management======*/
    List<RectTransform> cardPositions = new List<RectTransform>();
    GameObject[] cardTiles;
    Card[] cards;
    
    /*=========Transition=========*/
    Coroutine transitionRoutine;
    RectTransform rectTransform;
    Vector3 origin;
    Vector3 displayed;
    bool isOut;
    
    /*===============================
    *  Lifecycle
    ==============================*/
    void Awake()
    {
        cardPositions = GameObject
            .FindGameObjectsWithTag(Environment.TAG_CARD_PLACE_HOLDER)
            .Select(g => g.GetComponent<RectTransform>())
            .Where(rt => rt != null)
            .ToList();
        cardTiles = new GameObject[cardPositions.Count];
        cards = new Card[cardPositions.Count];
        
        rectTransform = GetComponent<RectTransform>();
        origin = rectTransform.position;
        displayed = new Vector3(origin.x + 190f, origin.y, origin.z);
    }

    /*===============================
    *  Card Management
    ==============================*/
    public bool AddCard()
    {
        var obj = Resources.Load(Environment.RESOURCE_CARD) as GameObject;

        var i = 0;
        while (i < cardPositions.Count)
            if (cards[i] == null) break;
            else i++;
        
        if (i == cardPositions.Count) return false;

        obj = Instantiate(obj, cardPositions[i]);
        obj.name = "card" + obj.GetInstanceID();
        cardTiles[i] = obj;
        cards[i] = Card.BlankCard();
        return true;
    }

    public bool RemoveCard()
    {
        var i = 0;
        while (i < cardPositions.Count)
            if (cards[i] != null) break;
            else i++;
        
        if (i == cardPositions.Count) return false;

        var tile = cardTiles[i];
        cardTiles[i] = null;
        cards[i] = null;
        Destroy(tile);
        return true;
    }
    
    /*===============================
    *  Transition
    ==============================*/
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transitionRoutine != null) StopCoroutine(transitionRoutine);
        transitionRoutine = StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        var start = rectTransform.position;
        var target = isOut ? origin : displayed;
        var duration = Environment.UI_CARD_SLIDE_OUT;
        var t = 0f;

        isOut = !isOut;

        while (t < duration)
        {
            rectTransform.position = new Vector3(
                Mathf.Lerp(start.x, target.x, t / duration),
                Mathf.Lerp(start.y, target.y, t / duration),
                Mathf.Lerp(start.z, target.z, t / duration));
            t += Time.deltaTime;
            yield return null;
        }


        rectTransform.position = target;
    }
}
