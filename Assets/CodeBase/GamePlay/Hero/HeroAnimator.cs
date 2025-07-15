using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroAnimator : MonoBehaviour
    {
        private const string IsMoving = "IsMoving";
        private const string AttackTrigger = "Attack";
        private const float MovementTreshold = 0.05f;

        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;

        private void Update()
        {
            animator.SetBool(IsMoving, characterController.velocity.magnitude >= MovementTreshold);
        }

        public void Attack() => animator.SetTrigger(AttackTrigger);
    }
}
