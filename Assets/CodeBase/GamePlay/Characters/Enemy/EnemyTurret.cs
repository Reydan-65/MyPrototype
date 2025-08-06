using CodeBase.Configs;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyTurret : Turret, IEnemyConfigInstaller
    {
        public void InstallEnemyConfig(EnemyConfig config)
        {
            turretType = config.TurretType;
            shootingRate = config.ShootingRate;
        }
    }
}