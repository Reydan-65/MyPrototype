using UnityEngine;

namespace CodeBase.GamePlay
{
    public class FirePointStabilizer : MonoBehaviour
    {
        [SerializeField] private Transform[] firePoints;
        [SerializeField] private Transform visualModel;
        [SerializeField] private bool stabilizeYPosition = true;
        [SerializeField] private bool stabilizeRotation = true;

        public void Stabilize()
        {
            if (firePoints == null || visualModel == null) return;

            for (int i = 0; i < firePoints.Length; i++)
            {
                if (firePoints[i] == null) continue;

                if (stabilizeYPosition)
                {
                    firePoints[i].position = new Vector3(
                        firePoints[i].position.x,
                        visualModel.position.y,
                        firePoints[i].position.z
                    );
                }

                if (stabilizeRotation)
                {
                    float currentYRotation = firePoints[i].eulerAngles.y;
                    firePoints[i].rotation = Quaternion.Euler(0, currentYRotation, 0);
                }
            }
        }

        public void ResetLocalRotation()
        {
            for (int i = 0; i < firePoints.Length; i++)
                firePoints[i].localRotation = Quaternion.identity;
        }
    }
}