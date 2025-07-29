using UnityEngine;

namespace CodeBase.GamePlay
{
    public class VisualModelTilt : MonoBehaviour
    {
        [SerializeField] private Transform visualModel;
        [SerializeField] private float maxTiltAngle;
        [SerializeField] private float tiltSpeed;
        [SerializeField] private float returnSpeed;

        private Vector3 lastMovementDirection;
        private float currentForwardTilt;
        private float currentSideTilt;

        public void UpdateTilt(Vector3 movementDirection)
        {
            if (visualModel == null) return;

            if (movementDirection.magnitude > 0.1f)
                lastMovementDirection = movementDirection.normalized;

            Vector3 localMoveDir = visualModel.InverseTransformDirection(lastMovementDirection);

            float speed = (movementDirection.magnitude > 0.1f) ? tiltSpeed : returnSpeed;
            float targetForwardTilt = 0f;
            float targetSideTilt = 0f;

            if (movementDirection.magnitude > 0.1f)
            {
                targetForwardTilt = localMoveDir.z * maxTiltAngle;
                targetSideTilt = -localMoveDir.x * maxTiltAngle;
            }

            currentForwardTilt = Mathf.Lerp(currentForwardTilt, targetForwardTilt, speed * Time.deltaTime);
            currentSideTilt = Mathf.Lerp(currentSideTilt, targetSideTilt, speed * Time.deltaTime);

            visualModel.localRotation = Quaternion.Euler(currentForwardTilt, 0, currentSideTilt);
        }
    }
}