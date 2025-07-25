using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.GamePlay.Hero
{
    public class HeroHealth : HeroResource, IHealth, IProgressLoadHandler
    {
        public bool IsDead => CheckDead();
        public void ApplyDamage(float damage) => ChangeValue(damage);
        public void ApplyHeal(float amount) => RestoreResource(amount);
        public void SetInvulnerability(bool enable) => SetImmune(enable);

        public void LoadProgress(PlayerProgress progress)
        {
            max = progress.HeroStats.MaxHitPoints;
            current = max;
            InvokeChangedEvent();
        }
    }
}