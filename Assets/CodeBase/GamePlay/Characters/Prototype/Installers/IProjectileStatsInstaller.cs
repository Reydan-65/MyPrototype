using CodeBase.Data;

namespace CodeBase.GamePlay.Projectile.Installer
{
    public interface IProjectileStatsInstaller
    {
        void InstallProjectileStats(ProjectileTypeStats stats, bool isPlayerProjectile);
    }
}
