using UnityEngine;
using UnityEditor;
using CodeBase.GamePlay.Interactive;

namespace CodeBase.Editor
{
    public class LootIDGenerator
    {
        [MenuItem("Tools/Generate Loot ID`s")]
        public static void GenerateLootIDs()
        {
            LootItem[] lootItems = Object.FindObjectsOfType<LootItem>();

            if (lootItems.Length == 0) return;

            foreach (LootItem item in lootItems)
            {
                item.GenerateID();
            }
        }
    }
}
