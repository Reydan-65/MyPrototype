using CodeBase.Configs;
using CodeBase.GamePlay.Turrets;
using CodeBase.Infrastructure.Services;

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