namespace CodeBase.GamePlay.Interactive
{
    public class CoinLoot : LootItem
    {
        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.HeroInventoryData != null)
                progressProvider.PlayerProgress.HeroInventoryData.AddCoin(1);
        }
    }
}
