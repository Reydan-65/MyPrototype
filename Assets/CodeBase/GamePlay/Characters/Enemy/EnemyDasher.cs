using CodeBase.Configs;
using CodeBase.GamePlay.Prototype;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Sounds;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyDasher : MonoBehaviour, IEnemyConfigInstaller
    {
        public SFXEvent PlaySFX;

        private float dashDistance;
        private float dashDuration;
        private float dashCooldown;
        private float aimDetectionAngle;
        private float detectionRange;
        private AnimationCurve dashCurve;

        private NavMeshAgent agent;
        private IHealth health;
        private Transform target;
        private EnemyShooter shooter;
        private PrototypeTurret targetTurret;
        private DashController dashController;

        public bool IsDashing => dashController?.IsDashing ?? false;

        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(IAssetProvider assetProvider) => this.assetProvider = assetProvider;

        private void Awake()
        {
            shooter = GetComponent<EnemyShooter>();

            if (shooter != null)
                shooter.TargetFound += OnTargetFound;
        }

        private void OnDestroy()
        {
            if (shooter != null)
                shooter.TargetFound -= OnTargetFound;
        }

        private void OnTargetFound()
        {
            if (target != null) return;

            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<IHealth>();
            target = shooter.Target;
            targetTurret = target.GetComponent<PrototypeTurret>();

            if (agent != null || health != null || target != null || targetTurret != null)
                Initialize(agent, health, target, targetTurret);
        }

        public void Initialize(NavMeshAgent agent, IHealth health, Transform target, PrototypeTurret targetTurret)
        {
            this.agent = agent;
            this.health = health;
            this.target = target;
            this.targetTurret = targetTurret;

            IMovementController movementController = new NavMeshMovementController(agent);
            dashController = new DashController(
                movementController,
                health,
                dashDistance,
                dashDuration,
                dashCooldown,
                dashCurve
            );
        }

        public async void TryDashIfAimed()
        {
            if (Time.frameCount % 2 != 0) return;

            if (!dashController.CanDash || target == null || targetTurret == null) return;
            if (!targetTurret.IsAiming || !targetTurret.IsFiring) return;

            Vector3 toEnemy = transform.position - target.position;
            float distanceToEnemy = toEnemy.magnitude;
            if (distanceToEnemy > detectionRange) return;

            float angleToEnemy = Vector3.Angle(target.forward, toEnemy.normalized);
            if (angleToEnemy < aimDetectionAngle)
            {
                PlaySFX?.Invoke(await assetProvider.Load<AudioClip>(AssetAddress.DashSound), 1, 1, 1);

                dashController.StartDash(CalculateDashDirection(toEnemy));
            }
        }

        private Vector3 CalculateDashDirection(Vector3 toEnemy)
        {
            float random = Random.value;
            Vector3 right = Vector3.Cross(toEnemy.normalized, Vector3.up);
            if (random < 0.4f) return right.normalized;
            if (random < 0.8f) return -right.normalized;
            return -toEnemy.normalized;
        }

        public void UpdateDash() => dashController.UpdateDash(Time.deltaTime, Vector3.zero);
        public void UpdateCooldown() => dashController.UpdateCooldown(Time.deltaTime);

        public void InstallEnemyConfig(EnemyConfig config)
        {
            dashDistance = config.DashDistance;
            dashDuration = config.DashDuration;
            dashCooldown = config.DashCooldown;
            aimDetectionAngle = config.AimDetectionAngle;
            detectionRange = config.DetectionRange;
            dashCurve = config.DashCurve;
        }
    }
}
