using CodeBase.Configs;
using CodeBase.GamePlay.Projectile.Installer;
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

        public GameObject CreateHitedImpactEffect()
        {
            if (hitSFX != null)
                return Instantiate(hitSFX, transform.position, Quaternion.identity);

            return null;
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            missSFX = config.MissSFXPrefab;
            hitSFX = config.HitSFXPrefab;
        }
    }
}
