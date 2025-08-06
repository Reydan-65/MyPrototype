using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyVisualRotation : MonoBehaviour
    {
        [SerializeField] private EnemyFollowToTarget followToTarget;
        [SerializeField] private EnemyShooter shooter;
        [SerializeField] private float rotationSpeed;

        private void Update()
        {
            if (followToTarget == null || shooter == null) return;

            Vector3 targetDirection = shooter.IsTargetInShootingRange() 
                ? followToTarget.GetDirectionToTarget() 
                : followToTarget.GetMovementDirection();

            if (targetDirection == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
