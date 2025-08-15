using CodeBase.Configs;
using CodeBase.GamePlay.Projectile.Installer;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileDestroyer : MonoBehaviour, IProjectileConfigInstaller
    {
        private GameObject missSFX;
        private GameObject hitSFX;

        private IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
        }

        public void CreateMissedImpactEffect()
        {
            if (missSFX != null)
                 gameFactory.CreateImpactEffectObjectFromPrefab(missSFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        public GameObject CreateHitedImpactEffect()
        {
            if (hitSFX != null)
                return gameFactory.CreateImpactEffectObjectFromPrefab(hitSFX, transform.position, Quaternion.identity);

            return null;
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            missSFX = config.MissSFXPrefab;
            hitSFX = config.HitSFXPrefab;
        }
    }
}
