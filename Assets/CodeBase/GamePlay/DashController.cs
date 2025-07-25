using UnityEngine;

namespace CodeBase.GamePlay
{
    public class DashController
    {
        private CharacterController characterController;
        private IHealth health;
        private float dashRange;
        private float dashDuration;
        private float dashCooldown;
        private AnimationCurve speedCurve;

        private float dashTimer;
        private float cooldownTimer;
        private bool isDashing;
        private Vector3 dashDirection;

        public bool IsDashing => isDashing;
        public bool CanDash => cooldownTimer <= 0 && !isDashing;

        public DashController(
            CharacterController characterController,
            IHealth health,
            float dashRange,
            float dashDuration,
            float dashCooldown,
            AnimationCurve speedCurve)
        {
            this.characterController = characterController;
            this.health = health;
            this.dashRange = dashRange;
            this.dashDuration = dashDuration;
            this.dashCooldown = dashCooldown;
            this.speedCurve = speedCurve;
        }
        
        public void StartDash(Vector3 direction)
        {
            if (!CanDash || health.Current <= 0) return;

            isDashing = true;
            dashTimer = 0f;
            dashDirection = direction.normalized;
            health.SetInvulnerability(true);
        }

        public void UpdateDash(float deltaTime, Vector3 gravityVelocity)
        {
            if (!isDashing) return;

            dashTimer += deltaTime;

            float progress = Mathf.Clamp01(dashTimer / dashDuration);
            float speedMultiplier = speedCurve.Evaluate(progress);
            Vector3 movement = dashDirection * (dashRange * speedMultiplier * deltaTime);
            movement += gravityVelocity * deltaTime;

            characterController.Move(movement);

            if (progress >= 1f) EndDash();
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= deltaTime;
        }

        private void EndDash()
        {
            isDashing = false;
            cooldownTimer = dashCooldown;
            health.SetInvulnerability(false);
        }
    }
}
