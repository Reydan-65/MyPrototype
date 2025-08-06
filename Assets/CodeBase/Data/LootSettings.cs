using UnityEngine;

namespace CodeBase.Data
{
    [System.Serializable]
    public class LootSettings
    {
        [Range(0, 100)] public int DropChance;
        public int MinAmount;
        public int MaxAmount;

        public bool ShouldDrop => Random.Range(0, 100) < DropChance;
        public int RandomAmount => Random.Range(MinAmount, MaxAmount + 1);
    }
}
