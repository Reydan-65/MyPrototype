using CodeBase.Data;
using CodeBase.GamePlay;
using CodeBase.GamePlay.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Prefabs")]
        public AssetReferenceGameObject PrefabReference;
        public GameObject DestroySFX;

        [Header("Identificator")]
        public EnemyID EnemyID;

        [Header("Max Health")]
        public float MaxHitPoints;

        [Header("Movement")]
        public float MovementSpeed;
        public float StopDistance;

        [Header("Combat Movement")]
        public float CombatMoveRadius;
        public float CombatMoveInterval;

        [Header("Turret")]
        public TurretType TurretType;
        public float ShootingRate;

        [Header("Shooter")]
        public float ShootingRange;
        public float ShootingAngle;

        [Header("Loot Settings")]
        public LootSettings CoinLoot;
        public LootSettings PotionLoot;

        [Header("Dash Settings")]
        public float DashDistance = 5f;
        public float DashDuration = 0.3f;
        public float DashCooldown = 5f;
        public float AimDetectionAngle = 15f;
        public float DetectionRange = 15f;
        public AnimationCurve DashCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [Header("Difficulty Scaling")]
        public float HealthScalePerLevel = 0.25f;
    }
}
