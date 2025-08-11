using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class KeyLoot : LootItem
    {
        [SerializeField] private string keyID;
        public string KeyID { get => keyID; set => keyID = value; }

        protected override void OnPickup()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
                progressProvider.PlayerProgress.PrototypeInventoryData.AddKey(KeyID);
        }
    }
}
