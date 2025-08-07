using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    [System.Serializable]
    public class PlayerProgress
    {
        public bool HasSavedGame = false;
        public int DifficultyIndex;
        public PrototypeStats PrototypeStats = new PrototypeStats();
        public PrototypeInventoryData PrototypeInventoryData = new PrototypeInventoryData();
        public PurchaseData PurchaseData = new PurchaseData();
        public PlayerSkinID PrototypeSkinID;

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

            progress.HasSavedGame = false;
            progress.DifficultyIndex = 0;
            progress.PrototypeStats = PrototypeStats.Default;
            progress.PrototypeInventoryData.SetDefaultInventoryData();
            progress.PurchaseData = new PurchaseData();
            progress.PrototypeSkinID = PlayerSkinID.BaseShip;

            progress.PickedLootItems.Clear();
            progress.InteractiveKeys.Clear();
            progress.InteractiveValues.Clear();
            progress.InteractiveStates.Clear();

            Debug.Log($"PROGRESS LOADED: DEFAULT!");

            return progress;
        }

        public void CopyFrom(PlayerProgress progress)
        {
            HasSavedGame = progress.HasSavedGame;
            DifficultyIndex = progress.DifficultyIndex;
            PrototypeStats.CopyFrom(progress.PrototypeStats);
            PrototypeInventoryData.CopyFrom(progress.PrototypeInventoryData);
            PurchaseData.CopyFrom(progress.PurchaseData);
            PrototypeSkinID = progress.PrototypeSkinID;

            PickedLootItems = new List<string>(progress.PickedLootItems);
            InteractiveKeys = new List<string>(progress.InteractiveStates.Keys);
            InteractiveValues = new List<bool>(progress.InteractiveStates.Values);
        }

        public void ResetProgress()
        {
            CopyFrom(GetDefaultProgress());
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