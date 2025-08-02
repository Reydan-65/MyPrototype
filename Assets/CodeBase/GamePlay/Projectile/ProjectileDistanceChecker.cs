using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileDistanceChecker : MonoBehaviour, IProjectileConfigInstaller
    {
        private float maxDistance = 0;

        private Vector3 spawnPosition;
        private ProjectileDestroyer destroyer;

        private void Start()
        {
            spawnPosition = transform.position;
            destroyer = GetComponent<ProjectileDestroyer>();
        }

        private void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            float currentDistance = Vector3.Distance(spawnPosition, transform.position);

            if (currentDistance >= maxDistance)
                destroyer.CreateMissedImpactEffect();
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            maxDistance = config.MaxDistance;
        }
    }
}
