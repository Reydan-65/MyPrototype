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

        [Header("Enemy Projectile Config")]
        [Header("Type")]
        public ProjectileType ProjectileType;

        [Header("Projectile Settings")]
        public float Speed;
        public float Range;
        public float MinDamage;
        public float MaxDamage;

        [Header("Difficulty Scaling")]
        public float DamageScalePerLevel = 0.25f;

        public float GetEnemyDamageSpread()
        {
            float damage = Random.Range(MinDamage, MaxDamage);
            return damage;
        }
    }
}
