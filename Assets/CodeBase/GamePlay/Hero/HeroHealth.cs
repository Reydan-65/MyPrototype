using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroHealth : Health, IProgressLoadHandler
    {
        [SerializeField] private float restoredHealthAmount;
        [SerializeField] private float restoreDelay;

        private float restoreTimer;

        public void Initialize(float max)
        {
            this.max = max;
            current = max;
            InvokeChangedEvent();
        }

        private void Update()
        {
            restoreTimer += Time.deltaTime;

            if (restoreTimer >= restoredHealthAmount)
            {
                RestoreHealth(restoredHealthAmount);
                restoreTimer = 0;
            }
        }

        public void RestoreHealth(float value)
        {
            if (current == max || value == 0) return;

            current += value;

            if (current > max)
                current = max;

            InvokeChangedEvent();
        }

        

        public void LoadProgress(PlayerProgress progress)
        {
            max = progress.HeroStats.MaxHitPoints;
            current = max;
            InvokeChangedEvent();
        }
    }
}
