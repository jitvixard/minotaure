using System;
using UnityEngine;
using Environment = src.util.Environment;

namespace src
{
    public class CameraController : MonoBehaviour
    {
        Camera cam;

        GameObject levelBase;
        
        [Header("Look At")]
        [SerializeField] Transform lookTarget;
        [SerializeField] float lookHeightOffset = 0f;

        [Header("Camera Offset")]
        [SerializeField] float heightOffset = 6.5f;
        [SerializeField] float distanceFromTarget = 9f;
        
        void Awake()
        {
            //loading in variables
            cam = GetComponent<Camera>();
            
            //setting look (if required)
            lookTarget = lookTarget == null
                ? GameObject.FindWithTag(Environment.LOOK_AT).transform
                : lookTarget;
            levelBase = GameObject.Find(Environment.LEVEL_BASE);

            //initiates the look at
            LookAt();
        }

        void Update()
        {
            LookAt();
        }

        void LookAt()
        {
            var lookAtPos = lookTarget.position;
            transform.LookAt(new Vector3(
                lookAtPos.x,
                lookAtPos.y = lookHeightOffset,
                lookAtPos.z));

            var dist = CalculateDistances();
            var centre = lookTarget.position;
            transform.position = new Vector3(
                centre.x - dist,
                centre.y + heightOffset,
                centre.z - dist);
        }

        float CalculateDistances()
        {
            var x = distanceFromTarget;
            x *= x;
            x /= 2;
            return Mathf.Sqrt(x);
        }
    }
}
