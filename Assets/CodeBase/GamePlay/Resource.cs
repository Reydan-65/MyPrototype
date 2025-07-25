using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay
{
    public abstract class Resource : MonoBehaviour, IResource
    {
        public event UnityAction Changed;
        public event UnityAction Depleted;

        protected float max;
        protected float current;
        protected bool isImmune;

        public float Max => max;
        public float Current => current;

        protected virtual void Awake() { }

        public virtual void ChangeValue(float amount)
        {
            if (current == 0 || amount == 0) return;

            current -= amount;

            if (current <= 0)
            {
                current = 0;
                Depleted?.Invoke();
            }

            InvokeChangedEvent();
        }

        public void SetImmune(bool enable)
        {
            isImmune = enable;
        }

        protected void InvokeChangedEvent()
        {
            Changed?.Invoke();
        }
    }
}
