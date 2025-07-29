using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroInput : MonoBehaviour
    {
        [SerializeField] private HeroMovement movement;
        [SerializeField] private HeroHealth health;
        [SerializeField] private HeroEnergy energy;
        [SerializeField] private HeroTurret turret;

        private IInputService input;
        private ICursorService cursor;
        private IHealingService healing;

        [Inject]
        public void Construct(
            IInputService input,
            ICursorService cursor,
            IHealingService healing)
        {
            this.input = input;
            this.cursor = cursor;
            this.healing = healing;
        }

        private void Update()
        {
            if (health == null || health.IsDead) return;

            HandleMovement();
            HandleRotation();
            HandleShooting();
            HandleHealing();
            HandleDash();
        }

        private void HandleMovement()
        {
            if (movement != null)
                movement.SetMovementDirection(input.MovementAxis);
        }

        private void HandleRotation()
        {
            if (input.AimInput && cursor != null)
            {
                Vector3 cursorPos = cursor.GetWorldPositionOnPlane(transform.position, Vector3.up);
                Vector3 lookDir = cursorPos - transform.position;
                lookDir.y = 0;

                if (lookDir != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        private void HandleShooting()
        {
            if (turret != null && input.AttackInput && turret.CanFire())
            {
                turret.Fire();
            }
        }

        private void HandleHealing()
        {
            if (input.HealInput && healing != null)
                healing.TryHeal(LootItemID.HealingPotion);
        }

        private void HandleDash()
        {
            if (input.DashInput && movement != null && energy != null)
                movement.TryDash();
        }
    }
}