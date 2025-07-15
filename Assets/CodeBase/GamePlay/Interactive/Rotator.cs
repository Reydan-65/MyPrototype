using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform targetTransdorm;
        [SerializeField] private Vector3 speed;

        private void Update()
        {
            targetTransdorm.Rotate(speed * Time.deltaTime);
        }
    }
}
