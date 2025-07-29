using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay.Enemies;
using CodeBase.GamePlay.Hero;
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
        public event UnityAction HeroCreated;

        private DIContainer container;
        private IAssetProvider assetProvider;
        private IProgressSaver progressSaver;
        private IConfigsProvider configProvider;
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
            this.configProvider = configProvider;
            this.enemySpawnManager = enemySpawnManager;
        }

        public GameObject HeroObject { get; private set; }
        public VirtualJoystick VirtualJoystick { get; private set; }
        public FollowCamera FollowCamera { get; private set; }
        public HeroHealth HeroHealth { get; private set; }
        public HeroEnergy HeroEnergy { get; private set; }
        public HeroInventory HeroInventory { get; private set; }
        public List<GameObject> EnemiesObject { get; private set; } = new List<GameObject>();
        public List<GameObject> ProjectilesObject { get; private set; } = new List<GameObject>();
        public GameObject LootObject { get; private set; }

        public async Task WarmUp()
        {
            EnemyConfig[] enemyConfigs = configProvider.GetAllEnemiesConfigs();

            for (int i = 0; i < enemyConfigs.Length; i++)
                await assetProvider.Load<GameObject>(enemyConfigs[i].PrefabReference);
        }

        public async Task<GameObject> InstantiateAndInject(string address)
        {
            GameObject newGameObject = await Addressables.InstantiateAsync(address).Task;
            container.InjectToGameObject(newGameObject);
            return newGameObject;
        }

        #region Create Hero
        public async Task<GameObject> CreateHeroAsync(Vector3 position, Quaternion rotation)
        {
            string heroPath = AssetAddress.HeroMalePath;

            HeroObject = await InstantiateAndInject(heroPath);
            HeroObject.transform.SetPositionAndRotation(position, rotation);

            HeroHealth = HeroObject.GetComponent<HeroHealth>();
            HeroEnergy = HeroObject.GetComponent<HeroEnergy>();

            var progress = progressSaver.GetProgress();

            HeroHealth.Initialize(progress.HeroStats.MaxHitPoints);
            HeroEnergy.Initialize(progress.HeroStats.MaxEnergy);

            HeroHealth.SetImmune(false);

            HeroInventoryData inventoryData = progress.HeroInventoryData;
            HeroInventory = HeroObject.GetComponent<HeroInventory>();
            HeroInventory.SyncWithData(inventoryData);

            progressSaver.AddObject(HeroObject);

            HeroCreated?.Invoke();
            return HeroObject;
        }
        #endregion

        #region Create Miscellaneous
        //public async Task<VirtualJoystick> CreateJoystickAsync()
        //{
        //    GameObject joystickObject = await InstantiateAndInject(AssetAddress.VirtualJoystickPath);
        //    VirtualJoystick = joystickObject.GetComponent<VirtualJoystick>();
        //    return VirtualJoystick;
        //}

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
            EnemyConfig enemyConfig = configProvider.GetEnemyConfig(id);

            GameObject enemyPrefab = await assetProvider.Load<GameObject>(enemyConfig.PrefabReference);
            GameObject enemy = container.Instantiate(enemyPrefab);

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            enemy.GetComponentInChildren<HealthBar>().SetResource(health);
            enemy.GetComponentInChildren<HealthText>().SetResource(health);

            enemy.transform.position = position;

            var installers = enemy.GetComponentsInChildren<IEnemyConfigInstaller>();
            for (int i = 0; i < installers.Length; i++)
                installers[i].InstallEnemyConfig(enemyConfig);

            EnemiesObject.Add(enemy);

            enemySpawnManager.RegisterEnemy(enemy);

            return enemy;
        }
        #endregion

        #region Create Projectile
        public GameObject CreateProjectileObjectFromPrefab(ProjectileType type)
        {
            ProjectileConfig config = configProvider.GetProjectileConfig(type);

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
