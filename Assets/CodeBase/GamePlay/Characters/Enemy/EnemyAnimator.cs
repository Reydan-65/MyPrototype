using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private const string IsMoving = "IsMoving";
        private const string AttackTrigger = "Attack";
        private const float MovementTreshold = 0.05f;

        [SerializeField] private Animator animator;

        public void PlayAttack() => animator.SetTrigger(AttackTrigger);
        public void SetMove(bool move) => animator.SetBool(IsMoving, move);
    }
}
