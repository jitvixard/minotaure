using System.Collections;
using UnityEngine;

namespace src.card.behaviours
{
    public abstract class CardBehaviour : MonoBehaviour
    {
        Coroutine   routine;
        public bool IsRunning => routine != null;

        void OnDisable()
        {
            Stop();
        }

        void OnDestroy()
        {
            Stop();
        }

        /**************** Card Behaviour ****************/
        protected abstract IEnumerator BehaviourRoutine();


        /*===============================
        *  Lifecycle
        ==============================*/
        public bool Play()
        {
            routine = StartCoroutine(BehaviourRoutine());
            return IsRunning;
        }

        public bool Stop()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                return true;
            }

            return false;
        }
    }
}