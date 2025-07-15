using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyAnimateByNavMeshAgent : MonoBehaviour
    {
        private const float Treshold = 0.05f;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private EnemyAnimator animator;

        private void Update()
        {
            animator.SetMove(agent.velocity.magnitude >= Treshold);
        }
    }
}
