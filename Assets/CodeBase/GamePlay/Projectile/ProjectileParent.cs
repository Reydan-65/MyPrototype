using CodeBase.GamePlay.Prototype;
using UnityEngine;

namespace CodeBase.GamePlay.Projectile
{
    public class ProjectileParent : MonoBehaviour
    {
        public Transform Parent { get; private set; }
        public bool IsPlayerProjectile { get; private set; }

        public void SetParent(Transform parent)
        {
            Parent = parent;
            IsPlayerProjectile = parent.GetComponentInChildren<PrototypeTurret>() != null;
        }
    }
}
