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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var point = IOHandler.ScreenClickToViewportPoint(screenTransform, Camera.main);
        var viewRay = castingCamera.ViewportPointToRay(point);
        if (Physics.Raycast(viewRay, out var hit))
        {
            print("hit " + hit.collider.name);
        }
        
        


        //TODO convert screenHit to camera co-ords of screenCamera
        //TODO raycast from screenCamera
        //TODO move agent
    }
}
