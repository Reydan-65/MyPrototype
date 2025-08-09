using CodeBase.Data;
using CodeBase.GamePlay.Projectile;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] protected Transform firePoint;
        [SerializeField] private ProjectileType projectileType;

        private ProjectileStats projectileStats;
        protected TurretType turretType;
        protected float shootingRate;

        protected float timer;
        protected IGameFactory gameFactory;
        public Transform FirePoint => firePoint;

        public bool IsFiring = false;

        [Inject]
        public void Construct(IGameFactory gameFactory, IProgressSaver progressSaver)
        {
            this.gameFactory = gameFactory;
            projectileStats = progressSaver.GetProgress().ProjectileStats;
        }

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