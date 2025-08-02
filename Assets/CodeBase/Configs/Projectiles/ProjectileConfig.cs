using CodeBase.GamePlay.Projectile;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Projectile")]
    public class ProjectileConfig : ScriptableObject
    {
        [Header("Prefab")]
        public GameObject ProjectilePrefab;
        public GameObject MissSFXPrefab;
        public GameObject HitSFXPrefab;

        [Header("Type")]
        public ProjectileType ProjectileType;

        [Header("Movement")]
        public float MovementSpeed;
        public float MaxDistance;

        [Header("Offensive")]
        public float Distance;
        public float MinDamage;
        public float MaxDamage;

        public float GetDamageSpread()
        {
            float damage = Random.Range(MinDamage, MaxDamage);
            return damage;
        }
    }
}
