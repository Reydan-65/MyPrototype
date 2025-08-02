using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    [System.Serializable]
    public class PlayerProgress
    {
        public int CurrentLevelIndex;
        public HeroStats HeroStats = new HeroStats();
        public HeroInventoryData HeroInventoryData = new HeroInventoryData();
        public PurchaseData PurchaseData = new PurchaseData();
        public HeroSkinID HeroSkinID;

        public List<string> PickedLootItems = new List<string>();

        public List<string> InteractiveKeys = new List<string>();
        public List<bool> InteractiveValues = new List<bool>();

        public Dictionary<string, bool> InteractiveStates
        {
            get
            {
                var dict = new Dictionary<string, bool>();
                for (int i = 0; i < InteractiveKeys.Count; i++)
                {
                    if (i < InteractiveValues.Count)
                        dict[InteractiveKeys[i]] = InteractiveValues[i];
                }
                return dict;
            }
            set
            {
                InteractiveKeys.Clear();
                InteractiveValues.Clear();
                foreach (var item in value)
                {
                    InteractiveKeys.Add(item.Key);
                    InteractiveValues.Add(item.Value);
                }
            }
        }

        public static PlayerProgress GetDefaultProgress()
        {
            var progress = new PlayerProgress();

            progress.CurrentLevelIndex = 0;
            progress.HeroStats = HeroStats.GetDefaultStats();
            progress.HeroInventoryData.SetDefaultInventoryData();
            progress.PurchaseData = new PurchaseData();
            progress.HeroSkinID = HeroSkinID.Male;

            progress.InteractiveKeys.Clear();
            progress.InteractiveValues.Clear();
            progress.InteractiveStates.Clear();

            Debug.Log($"PROGRESS LOADED: DEFAULT!");

            return progress;
        }

        public void CopyFrom(PlayerProgress progress)
        {
            CurrentLevelIndex = progress.CurrentLevelIndex;
            HeroStats.CopyFrom(progress.HeroStats);
            HeroInventoryData.CopyFrom(progress.HeroInventoryData);
            PurchaseData.CopyFrom(progress.PurchaseData);
            HeroSkinID = progress.HeroSkinID;

            PickedLootItems = new List<string>(progress.PickedLootItems);
            InteractiveKeys = new List<string>(progress.InteractiveStates.Keys);
            InteractiveValues = new List<bool>(progress.InteractiveStates.Values);
        }

        // Pecked Loot
        public void AddPickedLoot(string id)
        {
            if (!PickedLootItems.Contains(id))
                PickedLootItems.Add(id);
        }

        public bool WasLootPicked(string id) => PickedLootItems.Contains(id);

        // Interactable Objects
        public bool TryGetInteractiveState(string id, out bool state)
        {
            return InteractiveStates.TryGetValue(id, out state);
        }

        public void SetInteractiveState(string id, bool state)
        {
            var current = InteractiveStates;
            current[id] = state;
            InteractiveStates = current;
        }

        public void ClearInteractiveStates()
        {
            InteractiveStates.Clear();
        }
    }
}