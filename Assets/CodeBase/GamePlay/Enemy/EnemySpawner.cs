using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyID enemyID;

        private IGameFactory gameFactory;
        private EnemyLootDistributor lootDistributor;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
        }

        private void Awake()
        {
            lootDistributor = GetComponent<EnemyLootDistributor>();
        }

        public async void Spawn()
        {
            GameObject enemy = await gameFactory.CreateEnemyAsync(enemyID, transform.position);

            if (enemy == null) return;
            enemy.transform.rotation = transform.rotation;

            if (enemy.TryGetComponent(out EnemyInventory inventory))
                lootDistributor?.DistributeLoot(inventory);
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, 0.5f);

            Gizmos.color = Color.cyan;
            Vector3 direction = transform.forward * 1.5f;
            Gizmos.DrawLine(transform.position, transform.position + direction);
            Gizmos.DrawWireSphere(transform.position + direction, 0.1f);
        }

#endif
    }
}
