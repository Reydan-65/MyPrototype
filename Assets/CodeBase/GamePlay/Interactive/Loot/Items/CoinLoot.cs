using CodeBase.Data;

namespace CodeBase.GamePlay.Interactive
{
    public class CoinLoot : LootItem
    {
        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
            {
                progressProvider.PlayerProgress.PrototypeInventoryData.AddItem(LootItemID.Coin, 1);
            }
        }
    }
}
