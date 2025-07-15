using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileMovement : MonoBehaviour, IProjectileConfigInstaller
    {
        private float movementSpeed = 0;

        private void Update()
        {
            MoveProjectile();
        }

        private void MoveProjectile()
        {
            transform.position += transform.forward * (movementSpeed * Time.deltaTime);
        }

        public void InstallProjectileConfig(ProjectileConfig config)
        {
            movementSpeed = config.MovementSpeed;
        }
    }
}
