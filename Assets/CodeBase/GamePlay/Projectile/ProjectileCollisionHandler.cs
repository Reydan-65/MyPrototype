using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay.Enemies;
using CodeBase.GamePlay.Projectile.Installer;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileCollisionHandler : MonoBehaviour, IProjectileConfigInstaller, IProjectileStatsInstaller
    {
        private float damage = 0;
        private bool isPlayerProjectile;

        private Vector3 previousPosition;
        private ProjectileDestroyer destroyer;
        private ProjectileParent parent;
        private SphereCollider projectileCollider;

        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
            => this.progressProvider = progressProvider;

        private void Start()
        {
            previousPosition = transform.position;
            destroyer = GetComponent<ProjectileDestroyer>();
            projectileCollider = GetComponent<SphereCollider>();
            parent = GetComponent<ProjectileParent>();
        }

        private void Update()
        {
            OnHit();
            previousPosition = transform.position;
        }

        private void OnHit()
        {
            Vector3 direction = transform.position - previousPosition;
            float distance = direction.magnitude;

            if (distance > 0 &&
                Physics.SphereCast(previousPosition, projectileCollider.radius, direction.normalized, out RaycastHit hit, distance))
            {
                if (ShouldProcessCollision(hit.collider))
                    ProcessHit(hit.collider, hit.point);
            }
        }

        private void ProcessHit(Collider other, Vector3 hitPoint)
        {
            bool damageApplied = false;

            if (other.transform.root.TryGetComponent(out IHealth health))
            {
                if (!damageApplied)
                {
                    health.ApplyDamage((int)damage);
                    damageApplied = true;
                }
            }

            GameObject impactObject = destroyer.CreateHitedImpactEffect();
            impactObject.transform.position = hitPoint;
            impactObject.transform.SetParent(other.transform);

            Destroy(gameObject);
        }

        private bool ShouldProcessCollision(Collider other)
        {
            if (other == null || other.isTrigger || other.transform.root == parent)
                return false;

            if (!isPlayerProjectile)
            {
                if (other.transform.root.TryGetComponent(out EnemyHealth health))
                    return false;
            }

            return true;
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            if (!isPlayerProjectile)
            {
                damage = config.GetEnemyDamageSpread();
                damage += damage * config.DamageScalePerLevel * progressProvider.PlayerProgress.DifficultyLevel;
            }
        }

        public void InstallProjectileStats(ProjectileTypeStats stats, bool isPlayerProjectile)
        {
            this.isPlayerProjectile = isPlayerProjectile;

            if (isPlayerProjectile)
            {
                float spread = stats.AverageDamage.Value * 0.25f;
                damage = Random.Range(
                    stats.AverageDamage.Value - spread,
                    stats.AverageDamage.Value + spread
                );
                //Debug.Log($"AverageDamage.Value: {stats.AverageDamage.Value}, AverageDamage.Level:{stats.AverageDamage.Level}, damage: {damage}");
            }
        }
    }
}
