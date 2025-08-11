using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyHealth : Resource, IHealth, IEnemyConfigInstaller
    {
        public void ApplyDamage(float damage) => ChangeValue(damage);
        public void ApplyHeal(float amount) => ChangeValue(amount);
        public void SetInvulnerability(bool enable) => SetImmune(enable);

        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
            => this.progressProvider = progressProvider;

        public void InstallEnemyConfig(EnemyConfig config)
        {
            current = config.MaxHitPoints + (config.MaxHitPoints * config.HealthScalePerLevel* progressProvider.PlayerProgress.DifficultyLevel);
            max = config.MaxHitPoints + (config.MaxHitPoints * config.HealthScalePerLevel * progressProvider.PlayerProgress.DifficultyLevel);

            InvokeChangedEvent();
        }
    }
}
