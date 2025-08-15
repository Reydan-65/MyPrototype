using CodeBase.Data;
using CodeBase.GamePlay.Enemies;
using CodeBase.GamePlay.Projectile;
using CodeBase.GamePlay.Prototype;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Sounds;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        Task WarmUp();

        event UnityAction PrototypeCreated;
        GameObject PrototypeObject { get; }
        PrototypeHealth PrototypeHealth { get; }
        PrototypeInventory PrototypeInventory { get; }
        FollowCamera FollowCamera { get; }
        AudioPlayer AudioPlayer { get; }
        GameObject LootObject { get; }
        List<GameObject> EnemiesObject { get; }
        List<GameObject> ProjectilesObject { get; }

        Task<GameObject> CreatePrototypeAsync(Vector3 position, Quaternion rotation);
        Task<FollowCamera> CreateFollowCameraAsync();
        Task<AudioPlayer> CreateAudioPlayerAsync();
        Task<GameObject> CreateEnemyAsync(EnemyID id, Vector3 position);
        Task<GameObject> CreateLootItemFromPrefab(LootItemID id);
        GameObject CreateProjectileObjectFromPrefab(ProjectileType type, Transform parent);
        GameObject CreateImpactEffectObjectFromPrefab(GameObject impactobject, Vector3 position, Quaternion rotation);
    }
}
