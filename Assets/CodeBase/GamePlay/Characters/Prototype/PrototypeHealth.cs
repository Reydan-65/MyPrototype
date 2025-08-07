using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeHealth : PrototypeResource, IHealth, IProgressLoadHandler
    {
        private const float UPDATE_THRESHOLD = 0.033f;

        public event UnityAction Healed;
        public event UnityAction<float> HealTimerUpdated;

        private float healCooldown = 3f;
        private float timer;
        private float lastUpdateTime;

        public float HealCooldown => healCooldown;
        public float Timer { get => timer; set => timer = value; }

        public bool CanHeal => timer == 0;

        protected override void Update()
        {
            base.Update();

            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (Time.time - lastUpdateTime > UPDATE_THRESHOLD)
                {
                    HealTimerUpdated?.Invoke(timer);
                    lastUpdateTime = Time.time;
                }
            }
            else
            {
                timer = 0;
                HealTimerUpdated?.Invoke(0);
            }
        }

        public bool IsDead => CheckDead();
        public void ApplyDamage(float damage) => ChangeValue(damage);
        public void ApplyHeal(float amount)
        {
            RestoreResource(amount);
            Healed?.Invoke();
        }

        public void SetInvulnerability(bool enable) => SetImmune(enable);

        public void LoadProgress(PlayerProgress progress)
        {
            max = progress.PrototypeStats.Health.Value;
            current = max;
            InvokeChangedEvent();
        }
    }
}