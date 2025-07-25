using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public abstract class HeroResource : Resource
    {
        [SerializeField] protected float restoreAmountPerSecond;
        [SerializeField] protected float restoreDelay;

        private float timeSinceLastChange;

        public void Initialize(float max)
        {
            this.max = max;
            current = max;
            InvokeChangedEvent();
        }

        protected virtual void Update()
        {
            if (current >= max)
            {
                return;
            }

            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= restoreDelay)
            {
                float restoreAmount = restoreAmountPerSecond * Time.deltaTime;
                RestoreResource(restoreAmount);
            }
        }

        public override void ChangeValue(float amount)
        {
            base.ChangeValue(amount);
            timeSinceLastChange = 0f;
        }

        public void RestoreResource(float value)
        {
            if (current == max || value == 0) return;

            current += value;

            if (current > max)
                current = max;

            InvokeChangedEvent();
        }

        protected bool CheckDead()
        {
            if (current > 0) return false;
            return true;
        }
    }
}