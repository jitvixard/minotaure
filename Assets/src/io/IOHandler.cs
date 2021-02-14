using System;
using src.config.control;
using UnityEngine;

namespace src.io
{
    public class IOHandler : MonoBehaviour
    {

        ControlConfig control;

        void Awake()
        {
            control = ControlConfig.GetControl();
        }
        
        
        
    }
}