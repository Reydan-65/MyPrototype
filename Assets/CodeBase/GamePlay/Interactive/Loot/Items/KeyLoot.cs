using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class KeyLoot : LootItem
    {
        [SerializeField] private string KeyID;

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.HeroInventoryData != null)
                progressProvider.PlayerProgress.HeroInventoryData.AddKey(KeyID);
        }
    }
}
