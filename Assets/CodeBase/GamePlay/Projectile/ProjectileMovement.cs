using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay.Projectile.Installer;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileMovement : MonoBehaviour, IProjectileConfigInstaller, IProjectileStatsInstaller
    {
        private float movementSpeed = 0;
        private bool isPlayerProjectile;

        private void Update() => MoveProjectile();

        private void MoveProjectile()
            => transform.position += transform.forward * (movementSpeed * Time.deltaTime);

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            if (!isPlayerProjectile)
                movementSpeed = config.Speed;
        }

        public void InstallProjectileStats(ProjectileTypeStats stats, bool isPlayerProjectile)
        {
            if (isPlayerProjectile)
                movementSpeed = stats.Speed.Value;
        }
    }
}
