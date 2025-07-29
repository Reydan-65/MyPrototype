using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class FirePointStabilizer : MonoBehaviour
    {
        [SerializeField] private Transform[] firePoints;
        [SerializeField] private Transform visualModel;

        public void Stabilize()
        {
            if (firePoints == null || visualModel == null) return;

            for (int i = 0; i < firePoints.Length; i++)
            {
                if (firePoints[i] == null) continue;

                firePoints[i].position = new Vector3(firePoints[i].position.x,
                                                     visualModel.position.y,
                                                     firePoints[i].position.z);

                firePoints[i].rotation = Quaternion.Euler(0, firePoints[i].eulerAngles.y, 0);
            }
        }
    }
}