using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyCombatMovement : MonoBehaviour
    {
        [Header("Combat Settings")]
        [SerializeField] private float minMoveDelay = 1f;
        [SerializeField] private float maxMoveDelay = 3f;
        [SerializeField] private float stopingDistance = 1.5f;

        private NavMeshAgent agent;
        private Transform target;
        private Vector3 currentCombatPosition;
        private float radius;
        private float nextMoveTime;
        private bool isActive;

        public void SetActive(bool active) => isActive = active;

        public void Initialize(NavMeshAgent agent, Transform target, float radius)
        {
            this.agent = agent;
            this.target = target;
            this.radius = radius;

            isActive = true;
        }

        public void UpdateMovement()
        {
            if (!isActive || target == null) return;

            if (Time.time >= nextMoveTime)
            {
                CalculateNewCombatPosition();
                nextMoveTime = Time.time + Random.Range(minMoveDelay, maxMoveDelay);
            }

            if (Vector3.Distance(transform.position, currentCombatPosition) > stopingDistance)
                agent.destination = currentCombatPosition;
        }

        private void CalculateNewCombatPosition()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            currentCombatPosition = target.position +  new Vector3(randomDirection.x,0,randomDirection.y) * radius;

            if (NavMesh.SamplePosition(currentCombatPosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                currentCombatPosition = hit.position;
            }
        }
    }
}
