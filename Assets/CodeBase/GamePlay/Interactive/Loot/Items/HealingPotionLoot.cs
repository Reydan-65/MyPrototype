using CodeBase.Data;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class HealingPotionLoot : LootItem
    {
        [SerializeField] private float amountHeal = 15;

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
            {
                progressProvider.PlayerProgress.PrototypeInventoryData.AddHealingItem(LootItemID.HealingPotion, 1, amountHeal);
            }
        }
    }
}
