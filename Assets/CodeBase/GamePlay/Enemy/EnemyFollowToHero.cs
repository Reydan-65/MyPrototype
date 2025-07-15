using CodeBase.Configs;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyFollowToHero : MonoBehaviour, IEnemyConfigInstaller
    {
        [SerializeField] private NavMeshAgent agent;

        private float movementSpeed;
        private float stopDistance;
        private float combatMoveRadius;
        private float combatMoveInterval;

        private Vector3 lastDirection;
        private Vector3 currentCombatPosition;
        private float combatMoveTimer;

        private IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory) => this.gameFactory = gameFactory;

        private void Start()
        {
            agent.speed = movementSpeed;
            agent.stoppingDistance = stopDistance;
            agent.Warp(transform.position);
        }

        private void Update()
        {
            if (gameFactory.HeroObject == null) return;

            if (IsInCombatMode()) UpdateCombatMovement();
            else agent.destination = gameFactory.HeroObject.transform.position;

            if (agent.velocity.magnitude > 0.1f) lastDirection = agent.velocity.normalized;
        }

        private bool IsInCombatMode()
        {
            if (gameFactory.HeroObject == null) return false;
            float distance = Vector3.Distance(transform.position, gameFactory.HeroObject.transform.position);
            return distance <= stopDistance * 0.5f;
        }

        private void UpdateCombatMovement()
        {
            combatMoveTimer -= Time.deltaTime;

            if (combatMoveTimer <= 0 || Vector3.Distance(transform.position, currentCombatPosition) < 0.5f)
            {
                Vector2 randomCircle = Random.insideUnitCircle * combatMoveRadius;
                currentCombatPosition = gameFactory.HeroObject.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            }

            if (NavMesh.SamplePosition(currentCombatPosition, out NavMeshHit hit, combatMoveRadius, NavMesh.AllAreas))
            {
                currentCombatPosition = hit.position;
                agent.destination = currentCombatPosition;
                combatMoveTimer = combatMoveInterval;
            }
        }

        public Vector3 GetMovementDirection() => lastDirection;
        public Vector3 GetDirectionToTarget() => (gameFactory.HeroObject.transform.position - transform.position).normalized;

        public void InstallEnemyConfig(EnemyConfig config)
        {
            movementSpeed = config.MovementSpeed;
            stopDistance = config.StopDistance;
            combatMoveRadius = config.CombatMoveRadius;
            combatMoveInterval = config.CombatMoveInterval;
        }
    }
}
