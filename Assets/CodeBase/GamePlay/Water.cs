using UnityEngine;

namespace CodeBase.GamePlay
{
    public class Water : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                if (other.transform.root.TryGetComponent(out IHealth health))
                    health.ApplyDamage(health.Max);
            }
        }
    }
}
