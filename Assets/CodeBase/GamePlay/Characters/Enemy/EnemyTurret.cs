using CodeBase.Configs;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyTurret : BaseTurret, IEnemyConfigInstaller
    {
        public void InstallEnemyConfig(EnemyConfig config)
        {
            turretType = config.TurretType;
            fireRate = config.ShootingRate;
        }
    }
}