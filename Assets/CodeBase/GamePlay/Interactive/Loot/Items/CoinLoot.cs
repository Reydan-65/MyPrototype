using CodeBase.Data;

namespace CodeBase.GamePlay.Interactive
{
    public class CoinLoot : LootItem
    {
        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.HeroInventoryData != null)
            {
                progressProvider.PlayerProgress.HeroInventoryData.AddItem(LootItemID.Coin, 1);
            }
        }
    }
}
