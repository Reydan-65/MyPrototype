using CodeBase.Configs;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyHealth : Resource, IHealth, IEnemyConfigInstaller
    {
        public void ApplyDamage(float damage) => ChangeValue(damage);
        public void ApplyHeal(float amount) => ChangeValue(amount);
        public void SetInvulnerability(bool enable) => SetImmune(enable);

        public void InstallEnemyConfig(EnemyConfig config)
        {
            current = config.MaxHitPoints;
            max = config.MaxHitPoints;

            InvokeChangedEvent();
        }

        //public void ApplyCriticalDamage(float damage)
        //{
        //    ApplyDamage(damage * 1.5f);
        //}
    }
}
