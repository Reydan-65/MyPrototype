using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class KeyLoot : LootItem
    {
        [SerializeField] private string KeyID;

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
                progressProvider.PlayerProgress.PrototypeInventoryData.AddKey(KeyID);
        }
    }
}
