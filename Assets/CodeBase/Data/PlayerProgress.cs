using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    [System.Serializable]
    public class PlayerProgress
    {
        public bool HasSavedGame = false;
        public int DifficultyLevel;
        public PrototypeStats PrototypeStats = new PrototypeStats();
        public ProjectileStats ProjectileStats = new ProjectileStats();
        public PrototypeInventoryData PrototypeInventoryData = new PrototypeInventoryData();
        public PrototypeSkinID PrototypeSkinID;

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
            progress.DifficultyLevel = 0;
            progress.PrototypeStats = PrototypeStats.Default;
            progress.ProjectileStats = ProjectileStats.Default;
            progress.PrototypeInventoryData.SetDefaultInventoryData();
            progress.PrototypeSkinID = PrototypeSkinID.BaseSkin;

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
            DifficultyLevel = progress.DifficultyLevel;
            PrototypeStats.CopyFrom(progress.PrototypeStats);
            ProjectileStats.CopyFrom(progress.ProjectileStats);
            PrototypeInventoryData.CopyFrom(progress.PrototypeInventoryData);
            PrototypeSkinID = progress.PrototypeSkinID;

            PickedLootItems = new List<string>(progress.PickedLootItems);
            InteractiveKeys = new List<string>(progress.InteractiveStates.Keys);
            InteractiveValues = new List<bool>(progress.InteractiveStates.Values);
        }

        public void ResetProgress() => CopyFrom(GetDefaultProgress());
        public bool WasLootPicked(string id) => PickedLootItems.Contains(id);
        public bool TryGetInteractiveState(string id, out bool state) => InteractiveStates.TryGetValue(id, out state);
        public void ClearObjectsStates()
        {
            PickedLootItems.Clear();

            InteractiveStates.Clear();
            InteractiveValues.Clear();
            InteractiveKeys.Clear();
        }

        // Picked Loot
        public void AddPickedLoot(string id)
        {
            if (!PickedLootItems.Contains(id))
                PickedLootItems.Add(id);
        }

        public void SetInteractiveState(string id, bool state)
        {
            var current = InteractiveStates;
            current[id] = state;
            InteractiveStates = current;
        }
    }
}