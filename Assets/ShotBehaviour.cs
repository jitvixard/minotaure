using System.Collections;
using src.actors.controllers.impl;
using UnityEngine;
using Environment = src.util.Environment;

public class ShotBehaviour : MonoBehaviour
{
    Rigidbody rb;

    bool launched;

    float lifeTime;
    
    const int GraceFrames = 3; //if not launched in this amount of frames -> destroy
    int frameCount;
    
    
    
    /*===============================
    *  Lifecycle
    ==============================*/
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        name = "shot" + gameObject.GetInstanceID();

        StartCoroutine(PreLaunchRoutine());
    }

    IEnumerator PreLaunchRoutine()
    {
        while (!launched)
        {
            if (frameCount++ > GraceFrames && !launched)
                Destroy(gameObject);

            yield return null;
        }

        StartCoroutine(FlightRoutine());
    }

    IEnumerator FlightRoutine()
    {
        rb.velocity = transform.forward * Environment.PlayerService.ProjectileSpeed;
        
        const float lifeSpan = Environment.COMBAT_PROJECTILE_LIFE;
        
        while (lifeTime < lifeSpan)
        {
            lifeTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }


    /*===============================
    *  Handling
    ==============================*/
    public void Launch(RaycastHit hit)
    {
        var target = hit.point;
        var origin = transform.position;
        target.y = origin.y;

        var direction = target - origin;
        transform.rotation = Quaternion.LookRotation(direction);
        
        launched = true;
    }

    
    
    /*===============================
    *  Trigger
    ==============================*/
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Environment.TAG_SWARM))
            if (other.gameObject
                .TryGetComponent<SwarmActorController>(out var sac))
            {
                sac.Die();
                Destroy(gameObject);
            }
    }
}
