using CodeBase.Configs;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyShooter : MonoBehaviour, IEnemyConfigInstaller
    {
        public event UnityAction TargetFound;

        [SerializeField] private EnemyTurret[] turrets;

        private float shootingRange;
        private float shootingAngle;
        private Transform target;

        public Transform Target => target;

        private IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory) => this.gameFactory = gameFactory;

        private void OnEnable()
        {
            if (gameFactory != null)
                gameFactory.PrototypeCreated += OnPrototypeCreated;
        }

        private void OnDisable()
        {
            if (gameFactory != null)
                gameFactory.PrototypeCreated -= OnPrototypeCreated;
        }

        private void OnPrototypeCreated() => SetTarget(gameFactory.PrototypeObject.transform);

        private void Update()
        {
            if (target == null) return;

            foreach (var turret in turrets)
            {
                if (turret.CanFire() && IsTargetInShootingRange())
                    turret.Fire();
            }

            if (target.GetComponent<IHealth>().Current == 0)
                target = null;
        }

        public bool IsTargetInShootingRange()
        {
            if (target == null) return false;

            Vector3 targetPosXZ = new Vector3(target.position.x, transform.position.y, target.position.z);
            float distanceToTarget = Vector3.Distance(transform.position, targetPosXZ);
            return distanceToTarget <= shootingRange;
        }

        public void InstallEnemyConfig(EnemyConfig config)
        {
            shootingRange = config.ShootingRange;
            shootingAngle = config.ShootingAngle;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
            TargetFound?.Invoke();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;


            foreach (var turret in turrets)
            {
                if (turret != null && turret.FirePoint != null)
                {
                    Vector3 forward = turret.FirePoint.forward;
                    Vector3 leftBound = Quaternion.Euler(0, shootingAngle, 0) * forward * shootingRange;
                    Vector3 rightBound = Quaternion.Euler(0, -shootingAngle, 0) * forward * shootingRange;

                    Gizmos.DrawLine(turret.FirePoint.position, turret.FirePoint.position + leftBound);
                    Gizmos.DrawLine(turret.FirePoint.position, turret.FirePoint.position + rightBound);
                }
            }
        }
#endif
    }
}