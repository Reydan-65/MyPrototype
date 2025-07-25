using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileCollisionHandler : MonoBehaviour, IProjectileConfigInstaller
    {
        private float damage = 0;
        private GameObject destroySFX;

        private Transform parent;
        private Vector3 previousPosition;

        private void Start()
        {
            previousPosition = transform.position;
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

            if (distance > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(previousPosition, direction.normalized, out hit, distance))
                {
                    if (ShouldProcessCollision(hit.collider))
                    {
                        if (hit.transform.root.TryGetComponent(out IHealth health))
                            health.ApplyDamage((int)damage);

                        DestroyProjectile();
                    }
                }
            }
        }

        private bool ShouldProcessCollision(Collider other)
        {
            return other != null && other.transform.root != parent && !other.isTrigger;
        }

        private void DestroyProjectile()
        {
            if (destroySFX != null)
                Instantiate(destroySFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        public void SetParent(Transform parent)
        {
            this.parent = parent;
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            damage = config.Damage;
            destroySFX = config.DestroySFXPrefab;
        }
    }
}
