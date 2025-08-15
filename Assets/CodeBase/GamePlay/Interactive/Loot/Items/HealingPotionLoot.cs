using CodeBase.Data;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class HealingPotionLoot : LootItem
    {
        [SerializeField] private float amountHeal = 15;
        [SerializeField] private GameObject potionLootImpact;

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
                progressProvider.PlayerProgress.PrototypeInventoryData.AddHealingItem(LootItemID.HealingPotion, 1, amountHeal);
            if (gameFactory != null || potionLootImpact != null)
                gameFactory.CreateImpactEffectObjectFromPrefab(potionLootImpact, transform.position, Quaternion.identity);
        }
    }
}
