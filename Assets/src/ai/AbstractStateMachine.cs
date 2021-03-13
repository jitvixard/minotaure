using UnityEngine;

namespace src.ai
{
    public abstract class AbstractStateMachine : MonoBehaviour 
    {
        public abstract void UpdateState();
        public abstract void Idle();
        public abstract void Attack();
        public abstract void Regroup();
    }
}
