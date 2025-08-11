using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay.UI;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyDeath : BaseDeath, IEnemyConfigInstaller
    {
        private HealthBar healthBar;
        private EnemyInventory inventory;
        private EnemyFollowToTarget follower;
        private EnemyShooter shooter;
        private EnemyTargetPersuer persuer;
        private Collider enemyCollider;
        private ILootService lootService;

        [Inject]
        public void Construct(ILootService lootService) => this.lootService = lootService;

        protected override void Awake()
        {
            base.Awake();
            inventory = GetComponent<EnemyInventory>();
            healthBar = GetComponentInChildren<HealthBar>();
            follower = GetComponent<EnemyFollowToTarget>();
            shooter = GetComponent<EnemyShooter>();
            persuer = GetComponent<EnemyTargetPersuer>();
            enemyCollider = GetComponent<Collider>();
        }

        protected override async void OnDie()
        {
            gameFactory.CreateImpactEffectObjectFromPrefab(destroySFX, visualModel.transform.position, Quaternion.identity);
            base.OnDie();
            Destroy(healthBar.gameObject);
            if (inventory != null) await DropAllLoot();
        }

        private async Task DropAllLoot()
        {
            var lootTasks = new List<Task>();

            int coinsAmount = inventory.GetCoinsAmount();
            if (coinsAmount > 0)
                lootTasks.Add(lootService.DropLoot(transform.position, LootItemID.Coin, coinsAmount));

            int potionsAmount = inventory.GetHealingPotionsAmount();
            if (potionsAmount > 0)
                lootTasks.Add(lootService.DropLoot(transform.position, LootItemID.HealingPotion, potionsAmount));

            if (inventory.HasKey())
                lootTasks.Add(lootService.DropLoot(transform.position, LootItemID.Key, 1, inventory.KeyID));

            await Task.WhenAll(lootTasks);
        }

        protected override void DisableComponents()
        {
            follower.enabled = false;
            shooter.enabled = false;
            persuer.enabled = false;
            enemyCollider.enabled = false;
        }

        public void InstallEnemyConfig(EnemyConfig config) => destroySFX = config.DestroySFX;
    }
}
