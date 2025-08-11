using CodeBase.Configs;
using CodeBase.GamePlay.Prototype;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyFollowToTarget : MonoBehaviour, IEnemyConfigInstaller
    {
        private NavMeshAgent agent;
        private VisualModelTilt visualTilt;
        private FirePointStabilizer firePointStabilizer;
        private EnemyCombatMovement combatMovement;
        private EnemyDasher dasher;

        private float movementSpeed;
        private float stopDistance;
        private Vector3 lastDirection;
        private bool isInitialized;
        private EnemyConfig enemyConfig;

        private IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
            if (gameFactory.PrototypeObject == null)
                gameFactory.PrototypeCreated += OnPrototypeCreated;
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            visualTilt = GetComponent<VisualModelTilt>();
            firePointStabilizer = GetComponent<FirePointStabilizer>();
            combatMovement = GetComponent<EnemyCombatMovement>();
            dasher = GetComponent<EnemyDasher>();
        }

        public void Initialize(EnemyConfig config)
        {
            if (isInitialized) return;

            enemyConfig = config;
            InstallEnemyConfig(config);

            agent.speed = movementSpeed;
            agent.stoppingDistance = stopDistance;
            agent.Warp(transform.position);

            if (gameFactory.PrototypeObject != null)
                InitializeCombatMovement();

            isInitialized = true;
        }

        private void Update()
        {
            if (gameFactory.PrototypeObject == null) return;

            dasher.UpdateCooldown();
            dasher.UpdateDash();

            if (IsInCombatMode())
            {
                combatMovement.UpdateMovement();
                dasher.TryDashIfAimed();

                Vector3 toTarget = gameFactory.PrototypeObject.transform.position - transform.position;
                if (toTarget.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(toTarget.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }
            else
                agent.destination = gameFactory.PrototypeObject.transform.position;

            if (agent.velocity.magnitude > 0.1f)
                lastDirection = agent.velocity.normalized;

            visualTilt.UpdateTilt(lastDirection);
            firePointStabilizer.Stabilize();
        }

        private void OnPrototypeCreated()
        {
            if (isInitialized && combatMovement != null)
            {
                InitializeCombatMovement();
                InitializeDasher();
            }
            gameFactory.PrototypeCreated -= OnPrototypeCreated;
        }

        private void InitializeCombatMovement()
            => combatMovement.Initialize(agent, gameFactory.PrototypeObject.transform, enemyConfig.CombatMoveRadius);

        private void InitializeDasher()
        {
            GameObject target = gameFactory.PrototypeObject;
            PrototypeTurret targetTurret = target.transform.GetComponent<PrototypeTurret>();
            dasher.Initialize(agent, GetComponent<IHealth>(), target.transform, targetTurret);
        }

        private bool IsInCombatMode()
        {
            float distance = Vector3.Distance(transform.position, gameFactory.PrototypeObject.transform.position);
            return distance <= stopDistance * 1.2f;
        }

        public Vector3 GetMovementDirection()
            => agent.velocity.magnitude > 0.1f ? agent.velocity.normalized : lastDirection;

        public Vector3 GetDirectionToTarget()
            => (gameFactory.PrototypeObject.transform.position - transform.position).normalized;

        public void InstallEnemyConfig(EnemyConfig config)
        {
            movementSpeed = config.MovementSpeed;
            stopDistance = config.StopDistance;
        }
    }
}
