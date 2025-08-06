using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileCollisionHandler : MonoBehaviour, IProjectileConfigInstaller
    {
        private float damage = 0;

        private Transform parent;
        private Vector3 previousPosition;
        private ProjectileDestroyer destroyer;

        private SphereCollider projectileCollider;

        private void Start()
        {
            previousPosition = transform.position;
            destroyer = GetComponent<ProjectileDestroyer>();
            projectileCollider = GetComponent<SphereCollider>();
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

        public void SetParent(Transform parent) => this.parent = parent;

        public void InstallProjectileConfig(ProjectileConfig config)
            => damage = config.GetDamageSpread();
    }
}
