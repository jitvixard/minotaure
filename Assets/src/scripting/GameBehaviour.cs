using src.services;
using src.util;
using UnityEngine;

namespace src.scripting
{
    public class GameBehaviour : MonoBehaviour
    {
        /*===============================
         *  LifeCycle
         ==============================*/
        void Awake()
        {
            Environment.Init();
        }
    }
}
