using UnityEngine;
using Environment = src.util.Environment;

public class LightController : MonoBehaviour
{
    float rotationalSpeed;
    
    void Awake()
    {
        rotationalSpeed = Environment.FX_CAMERA_ROTATION;
    }

    void Update()
    {
        var delta = rotationalSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, delta,Space.World);
    }
}
