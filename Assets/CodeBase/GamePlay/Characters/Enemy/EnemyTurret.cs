using CodeBase.Configs;
using CodeBase.GamePlay.Turrets;

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