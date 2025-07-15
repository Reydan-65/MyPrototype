using CodeBase.GamePlay.Enemies;
using CodeBase.GamePlay.Turrets;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Prefab")]
        public AssetReferenceGameObject PrefabReference;

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
    }
}
