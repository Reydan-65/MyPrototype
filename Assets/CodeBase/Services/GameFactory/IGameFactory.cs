using CodeBase.Data;
using CodeBase.GamePlay.Enemies;
using CodeBase.GamePlay.Hero;
using CodeBase.GamePlay.Projectile;
using CodeBase.Infrastructure.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Infrastructure.Services.Factory
{
    public interface IGameFactory : IService
    {
        Task WarmUp();

        event UnityAction HeroCreated;
        Task<GameObject> CreateHeroAsync(Vector3 position, Quaternion rotation);
        GameObject HeroObject { get; }
        HeroHealth HeroHealth { get; }
        HeroInventory HeroInventory { get; }

        Task<FollowCamera> CreateFollowCameraAsync();
        FollowCamera FollowCamera { get; }
        //Task<VirtualJoystick> CreateJoystickAsync();
        //VirtualJoystick VirtualJoystick { get; }

        List<GameObject> EnemiesObject { get; }
        Task<GameObject> CreateEnemyAsync(EnemyID id, Vector3 position);
        Task<GameObject> CreateLootItemFromPrefab(LootItemID id);
        public GameObject LootObject { get; }

        List<GameObject> ProjectilesObject { get; }
        GameObject CreateProjectileObjectFromPrefab(ProjectileType type);
    }
}
