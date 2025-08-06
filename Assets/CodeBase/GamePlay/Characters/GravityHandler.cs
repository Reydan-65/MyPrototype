using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class GravityHandler
    {
        private CharacterController characterController;
        private float gravityForce;
        private float groundCheckDistance;
        private LayerMask groundLayer;

        private Vector3 verticalVelocity;
        private bool isGrounded;

        public Vector3 VerticalVelocity => verticalVelocity;
        public bool IsGrounded => isGrounded;

        public GravityHandler(
            CharacterController characterController,
            float gravityForce,
            float groundCheckDistance,
            LayerMask groundLayer)
        {
            this.characterController = characterController;
            this.gravityForce = gravityForce;
            this.groundCheckDistance = groundCheckDistance;
            this.groundLayer = groundLayer;
        }

        public void UpdateGravity()
        {
            CheckGround();
            ApplyGravity();
        }

        private void CheckGround()
        {
            isGrounded = characterController.isGrounded;

            if (Physics.Raycast(
                characterController.transform.position + Vector3.up * 0.1f,
                Vector3.down,
                out RaycastHit hit,
                groundCheckDistance,
                groundLayer))
            {
                isGrounded = true;

                if (hit.distance < groundCheckDistance * 0.9f)
                {
                    characterController.Move(Vector3.down * (groundCheckDistance - hit.distance));
                }
            }
        }

        private void ApplyGravity()
        {
            if (isGrounded)
                verticalVelocity.y = -2f;
            else
                verticalVelocity.y -= gravityForce * Time.deltaTime;
        }
    }
}