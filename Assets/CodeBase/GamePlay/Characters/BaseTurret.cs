using CodeBase.GamePlay.Projectile;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Sounds;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class BaseTurret : MonoBehaviour
    {
        public SFXEvent PlaySFX;

        [SerializeField] protected Transform firePoint;
        [SerializeField] private ProjectileType projectileType;

        protected TurretType turretType;
        protected float fireRate;
        protected float timer;

        public bool IsFiring = false;
        public Transform FirePoint => firePoint;

        protected IGameFactory gameFactory;
        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(IGameFactory gameFactory, IAssetProvider assetProvider)
        {
            this.gameFactory = gameFactory;
            this.assetProvider = assetProvider;
        }

        protected virtual void Update()
        {
            if (timer >= fireRate) timer = fireRate;
            else timer += Time.deltaTime;
        }

        public bool CanFire() => timer >= fireRate;

        public async void Fire()
        {
            timer = 0;
            GameObject projectile = gameFactory.CreateProjectileObjectFromPrefab(projectileType, transform.root);

            if (projectile == null) return;

            projectile.GetComponent<ProjectileParent>().SetParent(transform.root);
            projectile.transform.SetPositionAndRotation(firePoint.transform.position, firePoint.transform.rotation);
            projectile.tag = "ToDestroy";

            BaseTurret[] turrets = GetComponents<BaseTurret>();

            if (this == turrets[0])
                PlaySFX?.Invoke(await assetProvider.Load<AudioClip>(AssetAddress.ShootSound), 0.25f, 0.95f, 1.05f);
        }
    }
}