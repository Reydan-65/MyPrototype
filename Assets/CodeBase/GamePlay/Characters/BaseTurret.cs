using CodeBase.GamePlay.Projectile;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class BaseTurret : MonoBehaviour
    {
        [SerializeField] protected Transform firePoint;
        [SerializeField] private ProjectileType projectileType;

        protected TurretType turretType;
        protected float shootingRate;
        protected float timer;

        public bool IsFiring = false;
        public Transform FirePoint => firePoint;

        protected IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory) => this.gameFactory = gameFactory;

        protected virtual void Update()
        {
            if (timer >= shootingRate) timer = shootingRate;
            else timer += Time.deltaTime;
        }

        public bool CanFire() => timer >= shootingRate;

        public void Fire()
        {
            timer = 0;
            GameObject projectile = gameFactory.CreateProjectileObjectFromPrefab(projectileType, transform.root);

            if (projectile == null) return;

            projectile.GetComponent<ProjectileParent>().SetParent(transform.root);
            projectile.transform.SetPositionAndRotation(firePoint.transform.position, firePoint.transform.rotation);
            projectile.tag = "ToDestroy";
        }
    }
}