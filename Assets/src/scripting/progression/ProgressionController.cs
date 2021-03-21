using System;
using src.ai.swarm;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.scripting.progression
{
    public class ProgressionController : MonoBehaviour
    {
        readonly SwarmService swarmService = Environment.SwarmService;
        
        /*===============================
         *  Unity Lifecycle
         ==============================*/
        void Awake()
        {
            swarmService.Init(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                swarmService.NextWave();
            }
        }
    }
}