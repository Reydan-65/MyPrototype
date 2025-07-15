using UnityEngine;

namespace CodeBase.GamePlay.Enemies
{
    public class EnemyLootDistributor : MonoBehaviour
    {
        [SerializeField] private int minCoins = 0;
        [SerializeField] private int maxCoins = 3;
        [SerializeField] private int healingPotions = 0;
        [SerializeField] private bool hasKey = false;

        public void DistributeLoot(EnemyInventory inventory)
        {
            if (inventory == null) return;

            inventory.SetCoinsAmount(Random.Range(minCoins, maxCoins + 1));
            inventory.SetHealingPotionsAmount(healingPotions);
            inventory.SetHasKey(hasKey);
        }
    }
}
