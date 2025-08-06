using CodeBase.Data;
using CodeBase.GamePlay.Prototype;
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
        private IWindowsProvider windowsProvider;
        private IProgressSaver progressSaver;

        private PrototypePreviewLogic prototypePreviewLogic;

        private MainMenuWindow window;
        public MainMenuWindow Window => window;

        private GameObject containerPanel;
        private GameObject warningPanel;
        private PlayerProgress progress;
        private IUIFactory uiFactory;

        public MainMenuPresenter(
            IGameStateSwitcher gameStateSwitcher,
            IProgressProvider progressProvider,
            IConfigsProvider configProvider,
            IWindowsProvider windowsProvider,
            IProgressSaver progressSaver,
            IUIFactory uiFactory)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressProvider = progressProvider;
            this.configProvider = configProvider;
            this.windowsProvider = windowsProvider;
            this.progressSaver = progressSaver;
            this.uiFactory = uiFactory;

            prototypePreviewLogic = Object.FindObjectOfType<PrototypePreviewLogic>();
        }

        public override void SetWindow(MainMenuWindow window)
        {
            this.window = window;

            prototypePreviewLogic?.UpdatePreview();

            //int currentLevelIndex = progressProvider.PlayerProgress.DifficultyIndex;
            //window.SetLevelIndex(currentLevelIndex);

            this.window.ContinueButtonClicked += OnContinueButtonClicked;
            this.window.NewGameButtonClicked += OnNewGameButtonClicked;
            this.window.SettingsButtonClicked += OnSettingsButtonClicked;
            this.window.QuitGameButtonClicked += OnQuitGameButtonClicked;

            this.window.YesButtonClicked += OnYesButtonClicked;
            this.window.NoButtonClicked += OnNoButtonClicked;

            warningPanel.SetActive(false);

            progress = progressSaver.GetProgress();

            if (progress != null)
                window.SetContinueButtonState(progress.HasSavedGame);

            window.CleanUped += OnCleanUped;
        }

        private void OnCleanUped()
        {
            window.ContinueButtonClicked -= OnContinueButtonClicked;
            window.NewGameButtonClicked -= OnNewGameButtonClicked;
            window.SettingsButtonClicked -= OnSettingsButtonClicked;
            window.QuitGameButtonClicked -= OnQuitGameButtonClicked;

            window.CleanUped -= OnCleanUped;
        }

        private void OnContinueButtonClicked()
        {
            gameStateSwitcher.Enter<LoadNextLevelState>();
        }

        private void OnSettingsButtonClicked()
        {
            windowsProvider.SetSourceWindow(WindowID.MainMenuWindow);
            windowsProvider.Open(WindowID.SettingsWindow);

            window.Close();
        }

        private void OnNewGameButtonClicked()
        {
            if (progress.HasSavedGame)
            {
                window.SetConfirmPanelState(window.ContainerPanel, window.ConfirmPanel, true);
            }
            else
            {
                Debug.Log("NEW GAME STARTED!");
                gameStateSwitcher.Enter<LoadNextLevelState>();
            }
        }

        private void OnYesButtonClicked()
        {
            PlayerPrefs.DeleteKey(ProgressSaver.ProgressKey);
            PlayerPrefs.Save();

            progressProvider.PlayerProgress.ResetProgress();

            //window.SetLevelIndex(progressProvider.PlayerProgress.CurrentLevelIndex);
            prototypePreviewLogic?.UpdatePreview();

            progressSaver.SaveProgress();

            Debug.Log("PROGRESS DELETED!");
            Debug.Log("NEW GAME STARTED!");

            gameStateSwitcher.Enter<LoadNextLevelState>();
        }

        private void OnNoButtonClicked()
        {
            window.SetConfirmPanelState(window.ContainerPanel, window.ConfirmPanel, false);
        }

        private void OnQuitGameButtonClicked()
        {
            OnCleanUped();

            Application.Quit();
        }
    }
}
