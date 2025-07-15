using CodeBase.Configs;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyHealth : Health, IEnemyConfigInstaller
    {
        public void InstallEnemyConfig(EnemyConfig config)
        {
            current = config.MaxHitPoints;
            max = config.MaxHitPoints;
        }
    }
}
