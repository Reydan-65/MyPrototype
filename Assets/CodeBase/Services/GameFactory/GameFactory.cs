using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay.Enemies;
using CodeBase.GamePlay.Prototype;
using CodeBase.GamePlay.Interactive;
using CodeBase.GamePlay.Projectile;
using CodeBase.GamePlay.UI;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace CodeBase.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactory
    {
        public event UnityAction PrototypeCreated;

        private DIContainer container;
        private IAssetProvider assetProvider;
        private IProgressSaver progressSaver;
        private IConfigsProvider configsProvider;
        private IEnemySpawnManager enemySpawnManager;

        public GameFactory(
            DIContainer container,
            IAssetProvider assetProvider,
            IProgressSaver progressSaver,
            IConfigsProvider configProvider,
            IEnemySpawnManager enemySpawnManager)
        {
            this.container = container;
            this.assetProvider = assetProvider;
            this.progressSaver = progressSaver;
            this.configsProvider = configProvider;
            this.enemySpawnManager = enemySpawnManager;
        }

        public GameObject PrototypeObject { get; private set; }
        public VirtualJoystick VirtualJoystick { get; private set; }
        public FollowCamera FollowCamera { get; private set; }
        public PrototypeHealth PrototypeHealth { get; private set; }
        public PrototypeEnergy PrototypeEnergy { get; private set; }
        public PrototypeInventory PrototypeInventory { get; private set; }
        public List<GameObject> EnemiesObject { get; private set; } = new List<GameObject>();
        public List<GameObject> ProjectilesObject { get; private set; } = new List<GameObject>();
        public GameObject LootObject { get; private set; }

        public async Task WarmUp()
        {
            EnemyConfig[] enemyConfigs = configsProvider.GetAllEnemiesConfigs();

            for (int i = 0; i < enemyConfigs.Length; i++)
                await assetProvider.Load<GameObject>(enemyConfigs[i].PrefabReference);
        }

        public async Task<GameObject> InstantiateAndInject(string address)
        {
            GameObject newGameObject = await Addressables.InstantiateAsync(address).Task;
            container.InjectToGameObject(newGameObject);
            return newGameObject;
        }

        #region Create Prototype
        public async Task<GameObject> CreatePrototypeAsync(Vector3 position, Quaternion rotation)
        {
            string prototypePath = AssetAddress.BaseSkinPath;

            PrototypeObject = await InstantiateAndInject(prototypePath);
            PrototypeObject.transform.SetPositionAndRotation(position, rotation);
            PrototypeObject.name = "Prototype";

            PrototypeHealth = PrototypeObject.GetComponent<PrototypeHealth>();
            PrototypeEnergy = PrototypeObject.GetComponent<PrototypeEnergy>();

            var progress = progressSaver.GetProgress();

            PrototypeHealth.Initialize(progress.PrototypeStats.Health.Value);
            PrototypeEnergy.Initialize(progress.PrototypeStats.Energy.Value);

            PrototypeHealth.SetImmune(false);

            PrototypeInventoryData inventoryData = progress.PrototypeInventoryData;
            PrototypeInventory = PrototypeObject.GetComponent<PrototypeInventory>();
            PrototypeInventory.SyncWithData(inventoryData);

            progressSaver.AddObject(PrototypeObject);

            PrototypeCreated?.Invoke();
            return PrototypeObject;
        }
        #endregion

        #region Create Miscellaneous
        public async Task<FollowCamera> CreateFollowCameraAsync()
        {
            GameObject followCameraObject = await InstantiateAndInject(AssetAddress.FollowCameraPath);
            FollowCamera = followCameraObject.GetComponent<FollowCamera>();
            return FollowCamera;
        }
        #endregion

        #region Create Loot Item
        public async Task<GameObject> CreateLootItemFromPrefab(LootItemID id)
        {
            string path = id switch
            {
                LootItemID.Coin => AssetAddress.CoinLootPath,
                LootItemID.Key => AssetAddress.KeyLootPath,
                LootItemID.HealingPotion => AssetAddress.HealingPotionLootPath,
                LootItemID.None => null,
                _ => null
            };

            GameObject loot = await InstantiateAndInject(path);

            AddFaderComponentIfNeeded(loot);

            LootObject = loot;

            return LootObject;
        }

        private void AddFaderComponentIfNeeded(GameObject lootObject)
        {
            bool needFader = false;

            if (lootObject.TryGetComponent(out CoinLoot coin)) needFader = true;
            else if (lootObject.TryGetComponent(out HealingPotionLoot potion)) needFader = true;

            if (needFader)
            {
                var fader = lootObject.AddComponent<LootFader>();
            }
        }
        #endregion

        #region Create Enemy
        public async Task<GameObject> CreateEnemyAsync(EnemyID id, Vector3 position)
        {
            EnemyConfig enemyConfig = configsProvider.GetEnemyConfig(id);
            PlayerProgress progress = progressSaver.GetProgress();
            GameObject enemyPrefab = await assetProvider.Load<GameObject>(enemyConfig.PrefabReference);
            GameObject enemy = container.Instantiate(enemyPrefab);

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            enemy.GetComponentInChildren<HealthBar>().SetResource(health);
            enemy.GetComponentInChildren<HealthText>().SetResource(health);

            enemy.transform.position = position;

            var installers = enemy.GetComponentsInChildren<IEnemyConfigInstaller>();
            for (int i = 0; i < installers.Length; i++)
            {
                enemyConfig.MaxHitPoints += progress.DifficultyIndex * 1.25f;
                installers[i].InstallEnemyConfig(enemyConfig);

                if (installers[i] is EnemyFollowToTarget followToTarget)
                    followToTarget.Initialize(enemyConfig);
            }

            EnemiesObject.Add(enemy);

            enemySpawnManager.RegisterEnemy(enemy);

            return enemy;
        }
        #endregion

        #region Create Projectile
        public GameObject CreateProjectileObjectFromPrefab(ProjectileType type)
        {
            ProjectileConfig config = configsProvider.GetProjectileConfig(type);

            GameObject projectilePrefab = config.ProjectilePrefab;
            GameObject projectileObject = container.Instantiate(projectilePrefab);

            var installers = projectileObject.GetComponentsInChildren<IProjectileConfigInstaller>();
            foreach (var installer in installers)
                installer.InstallProjectileConfig(config);

            ProjectilesObject.Add(projectileObject);

            return projectileObject;
        }
        #endregion

        //private GameObject CreateGameObjectFromPrefab(string prefabPath)
        //{
        //    GameObject prefab = assetProvider.GetPrefab<GameObject>(prefabPath);
        //    return container.Instantiate(prefab);
        //}

        //private T CreateComponentFromPrefab<T>(string prefabPath) where T : Component
        //{
        //    GameObject prefab = assetProvider.GetPrefab<GameObject>(prefabPath);
        //    GameObject obj = container.Instantiate(prefab);
        //    return obj.GetComponent<T>();
        //}
    }
}
