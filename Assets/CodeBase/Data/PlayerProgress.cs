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

        public static PlayerProgress GetDefaultProgress()
        {
            var progress = new PlayerProgress();

            progress.CurrentLevelIndex = 0;
            progress.HeroStats = HeroStats.GetDefaultStats();
            progress.HeroInventoryData.SetDefaultInventoryData();
            progress.PurchaseData = new PurchaseData();
            progress.HeroSkinID = HeroSkinID.Male;

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
        }
    }
}
