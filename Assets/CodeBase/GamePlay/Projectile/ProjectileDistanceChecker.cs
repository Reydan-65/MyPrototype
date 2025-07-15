using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileDistanceChecker : MonoBehaviour, IProjectileConfigInstaller
    {
        private float maxDistance = 0;

        private Vector3 spawnPosition;

        private void Start()
        {
            spawnPosition = transform.position;
        }

        private void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            float currentDistance = Vector3.Distance(spawnPosition, transform.position);

            if (currentDistance >= maxDistance)
                Destroy(gameObject);
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            maxDistance = config.MaxDistance;
        }
    }
}
