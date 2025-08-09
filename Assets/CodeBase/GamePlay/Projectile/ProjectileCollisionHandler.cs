using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay.Projectile.Installer;
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
            => other != null && other.transform.root != parent && !other.isTrigger;


        public void InstallProjectileConfig(ProjectileConfig config)
        {
            if (!isPlayerProjectile)
            {
                damage = Random.Range(config.MinDamage, config.MaxDamage);
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
            }
        }
    }
}
