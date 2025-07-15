using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class FinishPoint : MonoBehaviour, IService
    {
        public static float Radius = 3;

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.75f, 0.4f ,0, 0.5f);
            Gizmos.DrawSphere(transform.position, Radius);
        }

#endif
    }
}
