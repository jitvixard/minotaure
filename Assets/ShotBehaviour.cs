using System.Collections;
using System.Collections.Generic;
using src.util;
using UnityEngine;

public class ShotBehaviour : MonoBehaviour
{
    Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = transform.forward * Environment.COMBAT_PROJECTILE_SPEED;
        }
    }
}
