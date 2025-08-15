using CodeBase.Configs;
using CodeBase.GamePlay.Enemies;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Spawners
{
    public class LootDistributor : MonoBehaviour
    {
        [SerializeField] private bool hasKey = false;
        [SerializeField] private string keyID;
        public string KeyID { get => keyID; set => keyID = value; }

        private EnemyConfig enemyConfig;
        private EnemyID enemyId;

        private IProgressSaver progressSaver;

        [Inject]
        public void Construct(IProgressSaver progressSaver) => this.progressSaver = progressSaver;

        public void Initialize(IConfigsProvider configsProvider, EnemyID enemyId)
        {
            this.enemyId = enemyId;
            enemyConfig = configsProvider.GetEnemyConfig(this.enemyId);
        }

        public void DistributeLoot(EnemyInventory inventory)
        {
            if (inventory == null || enemyConfig == null) return;

            if (enemyConfig.CoinLoot.ShouldDrop)
                inventory.SetCoinsAmount(enemyConfig.CoinLoot.RandomAmount);
            else
                inventory.SetCoinsAmount(0);

            if (enemyConfig.PotionLoot.ShouldDrop)
                inventory.SetHealingPotionsAmount(enemyConfig.PotionLoot.RandomAmount);
            else
                inventory.SetHealingPotionsAmount(0);

            var progress = progressSaver.GetProgress();

            inventory.SetHasKey(hasKey);
        }
    }
}
