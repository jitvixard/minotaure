using System;
using System.Collections;
using System.Collections.Generic;
using src.io;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ScreenHandler : MonoBehaviour, IPointerClickHandler
{
    //cameras
    [SerializeField] Camera castingCamera;
    Camera mainCamera;
    //raycasting
    int filterLayer;

    NavMeshAgent agent;
    
    RectTransform screenTransform;

    void Awake()
    {
        agent = GameObject.Find("actor").GetComponent<NavMeshAgent>();
        
        mainCamera = Camera.main;
        filterLayer = castingCamera.gameObject.layer;
        
        screenTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var screenHit = IOHandler.ClickToScreenPoint(screenTransform, Camera.main);
        //use castingCamera.rect
        var rect = screenTransform.rect;
        var heightBound = rect.height / 2;
        var widthBound = rect.width / 2;
        var upperBounds = new Vector2(widthBound, heightBound);
        var lowerBounds = new Vector2(-widthBound, -heightBound);

        //TODO convert screenHit to camera co-ords of screenCamera
        //TODO raycast from screenCamera
        //TODO move agent
    }
}
