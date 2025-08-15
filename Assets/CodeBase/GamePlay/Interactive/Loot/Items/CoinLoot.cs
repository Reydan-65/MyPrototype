using CodeBase.Data;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class CoinLoot : LootItem
    {
        [SerializeField] private GameObject coinLootImpact;

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
                progressProvider.PlayerProgress.PrototypeInventoryData.AddItem(LootItemID.Coin, 1);

            if (gameFactory != null || coinLootImpact != null)
                gameFactory.CreateImpactEffectObjectFromPrefab(coinLootImpact, transform.position, Quaternion.identity);
        }
    }
}
