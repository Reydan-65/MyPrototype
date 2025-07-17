using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay
{
    public abstract class Health : MonoBehaviour
    {
        public event UnityAction Changed;
        public event UnityAction Die;

        protected float max;
        protected float current;
        protected bool invulnerability;

        public float Max => max;
        public float Current => current;

        protected virtual void Awake() { }

        public void ApplyDamage(float damage)
        {
            if (current == 0 || damage == 0) return;

            current -= damage;

            if (current <= 0)
            {
                current = 0;
                Die?.Invoke();
            }

            InvokeChangedEvent();
        }

        public void SetInvulnerability(bool enable)
        {
            invulnerability = enable;
        }

        protected void InvokeChangedEvent()
        {
            Changed?.Invoke();
        }
    }
}
