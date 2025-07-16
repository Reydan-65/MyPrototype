namespace CodeBase.GamePlay.Interactive
{
    public class KeyLoot : LootItem
    {
        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.HeroInventoryData != null)
                progressProvider.PlayerProgress.HeroInventoryData.HasKey = true;
        }
    }
}
