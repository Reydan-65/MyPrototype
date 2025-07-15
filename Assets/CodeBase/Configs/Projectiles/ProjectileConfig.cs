using CodeBase.GamePlay.Projectile;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Projectile")]
    public class ProjectileConfig : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject ProjectilePrefab;
        public GameObject DestroySFXPrefab;

        [Header("Type")]
        public ProjectileType ProjectileType;

        [Header("Movement")]
        public float MovementSpeed;
        public float MaxDistance;

        [Header("Offensive")]
        public float Distance;
        public float Damage;
    }
}
