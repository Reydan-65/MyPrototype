using CodeBase.Data;
using CodeBase.GamePlay.Hero;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class MainMenuPresenter : WindowPresenterBase<MainMenuWindow>
    {
        private IGameStateSwitcher gameStateSwitcher;
        private IProgressProvider progressProvider;
        private IConfigsProvider configProvider;
        //private IWindowsProvider windowsProvider;
        private IProgressSaver progressSaver;

        private HeroPreviewLogic heroPreviewLogic;
        private MainMenuWindow window;
        public MainMenuWindow Window => window;

        public MainMenuPresenter(
            IGameStateSwitcher gameStateSwitcher,
            IProgressProvider progressProvider,
            IConfigsProvider configProvider,
            //IWindowsProvider windowsProvider,
            IProgressSaver progressSaver)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressProvider = progressProvider;
            this.configProvider = configProvider;
            //this.windowsProvider = windowsProvider;
            this.progressSaver = progressSaver;

            heroPreviewLogic = Object.FindObjectOfType<HeroPreviewLogic>();
        }

        public override void SetWindow(MainMenuWindow window)
        {
            this.window = window;

            UpdateSkin();

            heroPreviewLogic?.UpdatePreview();

            int currentLevelIndex = progressProvider.PlayerProgress.CurrentLevelIndex;

            if (currentLevelIndex >= configProvider.LevelAmount)
                window.HideLevelButton();
            else
                window.SetLevelIndex(currentLevelIndex);

            window.PlayButtonClicked += OnPlayButtonClicked;
            //window.ShopButtonClicked += OnShopButtonClicked;
            window.ResetButtonClicked += OnResetButtonClicked;

            window.SelectMaleSkinButtonClicked += OnMaleSkinSelected;
            window.SelectFemaleSkinButtonClicked += OnFemaleSkinSelected;

            window.CleanUped += OnCleanUped;
        }

        private void OnCleanUped()
        {
            window.PlayButtonClicked -= OnPlayButtonClicked;
            //window.ShopButtonClicked -= OnShopButtonClicked;
            window.ResetButtonClicked -= OnResetButtonClicked;

            window.SelectMaleSkinButtonClicked -= OnMaleSkinSelected;
            window.SelectFemaleSkinButtonClicked -= OnFemaleSkinSelected;

            window.CleanUped -= OnCleanUped;
        }

        public void UpdateSkin()
        {
            var progress = progressProvider.PlayerProgress;
            bool isFemaleSkinUnlocked = progress.PurchaseData?.IsFemaleSkinUnlocked ?? false;

            window.SetSkinSelectionVisibility(isFemaleSkinUnlocked);

            if (isFemaleSkinUnlocked)
                window.UpdateSkinSelectionButtonsView(progress.HeroSkinID);

            heroPreviewLogic?.UpdatePreview();
        }

        private void OnMaleSkinSelected()
        {
            progressProvider.PlayerProgress.HeroSkinID = HeroSkinID.Male;
            UpdateSkin();
            progressSaver.SaveProgress();
        }

        private void OnFemaleSkinSelected()
        {
            progressProvider.PlayerProgress.HeroSkinID = HeroSkinID.Female;
            UpdateSkin();
            progressSaver.SaveProgress();
        }

        private void OnPlayButtonClicked()
        {
            gameStateSwitcher.Enter<LoadNextLevelState>();
        }

        //private void OnShopButtonClicked()
        //{
        //    window.Close();
        //    windowsProvider.Open(WindowID.ShopWindow);
        //}

        private void OnResetButtonClicked()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            progressProvider.PlayerProgress.CurrentLevelIndex = 0;
            progressProvider.PlayerProgress.HeroSkinID = HeroSkinID.Male;

            progressProvider.PlayerProgress.HeroInventoryData.SetDefaultInventoryData();
            progressProvider.PlayerProgress.PurchaseData = new PurchaseData();
            progressProvider.PlayerProgress.HeroStats.SetDefaultStats();

            UpdateSkin();
            window.SetLevelIndex(progressProvider.PlayerProgress.CurrentLevelIndex);
            heroPreviewLogic?.UpdatePreview();

            progressSaver.SaveProgress();

            Debug.Log("PROGRESS DELETED!");
        }
    }
}
