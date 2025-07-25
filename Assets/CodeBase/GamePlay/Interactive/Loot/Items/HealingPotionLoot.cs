using CodeBase.Data;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class HealingPotionLoot : LootItem
    {
        [SerializeField] private float amountHeal = 15;

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.HeroInventoryData != null)
            {
                progressProvider.PlayerProgress.HeroInventoryData.AddHealingItem(LootItemID.HealingPotion, 1, amountHeal);
            }
        }
    }
}
