using CodeBase.Data;
using CodeBase.GamePlay.Turrets;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.GamePlay.Hero
{
    public class HeroTurret : Turret, IProgressLoadHandler
    {
        public void Initialize(float value)
        {
            shootingRate = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            turretType = progress.HeroStats.TurretType;
            Initialize(progress.HeroStats.ShootingRate);
        }
    }
}