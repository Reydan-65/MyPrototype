using CodeBase.GamePlay.Enemies;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.Factory;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyID enemyID;

        private LootDistributor lootDistributor;

        private IGameFactory gameFactory;
        private IConfigsProvider configsProvider;
        private IEnemySpawnManager enemySpawnManager;

        [Inject]
        public void Construct(
            IGameFactory gameFactory,
            IConfigsProvider configsProvider,
            IEnemySpawnManager enemySpawnManager)
        {
            this.gameFactory = gameFactory;
            this.configsProvider = configsProvider;
            this.enemySpawnManager = enemySpawnManager;
        }

        private void Awake()
            => lootDistributor = GetComponent<LootDistributor>();

        public async Task Spawn()
        {
            GameObject enemy = await gameFactory.CreateEnemyAsync(enemyID, transform.position);

            if (enemy == null) return;
            enemy.transform.rotation = transform.rotation;

            if (lootDistributor != null)
            {
                lootDistributor.Initialize(configsProvider, enemyID);
                if (enemy.TryGetComponent(out EnemyInventory inventory))
                {
                    lootDistributor?.DistributeLoot(inventory);
                    inventory.KeyID = lootDistributor.KeyID;
                }
            }

            if (enemy.TryGetComponent(out EnemyShooter shooter))
            {
                if (gameFactory.PrototypeObject != null)
                    shooter.SetTarget(gameFactory.PrototypeObject.transform);
                else
                    gameFactory.PrototypeCreated += () => shooter.SetTarget(gameFactory.PrototypeObject.transform);
            }

            enemySpawnManager.RegisterEnemy(enemy);
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);

            Gizmos.color = Color.cyan;
            Vector3 direction = transform.forward * 1.5f;
            Gizmos.DrawLine(transform.position, transform.position + direction);
            Gizmos.DrawWireSphere(transform.position + direction, 0.1f);
        }

#endif
    }
}
