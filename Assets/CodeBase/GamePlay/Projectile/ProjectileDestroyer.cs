using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileDestroyer : MonoBehaviour, IProjectileConfigInstaller
    {
        private GameObject missSFX;
        private GameObject hitSFX;

        public void CreateMissedImpactEffect()
        {
            if (missSFX != null)
                Instantiate(missSFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        public void CreateHitedImpactEffect()
        {
            if (hitSFX != null)
                Instantiate(hitSFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            missSFX = config.MissSFXPrefab;
            hitSFX = config.HitSFXPrefab;
        }
    }
}
