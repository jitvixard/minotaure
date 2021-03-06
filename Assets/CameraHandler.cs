using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] new Camera camera;
    NavMeshAgent agent;
    
    void Awake()
    {
        agent = GameObject.Find("actor").GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("click");
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hit, 100f))
            {
                print("hit");
                agent.destination = hit.point;
            }
        }
    }
}
