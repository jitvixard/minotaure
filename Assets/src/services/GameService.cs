using src.scripting;
using src.util;
using UnityEngine;

namespace src.services
{
    public class GameService
    {
        GameBehaviour gameBehaviour;
        
        public void Init()
        {
            gameBehaviour = GameObject
                .FindGameObjectWithTag(Environment.TAG_MAIN_CAMERA)
                .GetComponent<GameBehaviour>();
        }
    }
}