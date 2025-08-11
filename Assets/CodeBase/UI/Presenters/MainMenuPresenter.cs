using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class MainMenuPresenter : WindowPresenterBase<MainMenuWindow>
    {
        private MainMenuWindow window;
        private GameObject warningPanel;
        private PlayerProgress progress;
        
        public MainMenuWindow Window => window;
        
        private IGameStateSwitcher gameStateSwitcher;
        private IProgressProvider progressProvider;
        private IWindowsProvider windowsProvider;
        private IProgressSaver progressSaver;

        public MainMenuPresenter(
            IGameStateSwitcher gameStateSwitcher,
            IProgressProvider progressProvider,
            IWindowsProvider windowsProvider,
            IProgressSaver progressSaver)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressProvider = progressProvider;
            this.windowsProvider = windowsProvider;
            this.progressSaver = progressSaver;
        }

        public override void SetWindow(MainMenuWindow window)
        {
            this.window = window;

            this.window.ContinueButtonClicked += OnContinueButtonClicked;
            this.window.NewGameButtonClicked += OnNewGameButtonClicked;
            this.window.SettingsButtonClicked += OnSettingsButtonClicked;
            this.window.QuitGameButtonClicked += OnQuitGameButtonClicked;
            this.window.YesButtonClicked += OnYesButtonClicked;
            this.window.NoButtonClicked += OnNoButtonClicked;
            this.window.CleanUped += OnCleanUped;

            progress = progressSaver.GetProgress();

            if (progress != null)
            {
                this.window.SetContinueButtonState(progress.HasSavedGame);
                this.window.SetDifficultyInfoText((progress.DifficultyLevel + 1).ToString());
            }

            warningPanel.SetActive(false);
        }


        private void OnCleanUped()
        {
            window.ContinueButtonClicked -= OnContinueButtonClicked;
            window.NewGameButtonClicked -= OnNewGameButtonClicked;
            window.SettingsButtonClicked -= OnSettingsButtonClicked;
            window.QuitGameButtonClicked -= OnQuitGameButtonClicked;
            window.YesButtonClicked -= OnYesButtonClicked;
            window.NoButtonClicked -= OnNoButtonClicked;
            window.CleanUped -= OnCleanUped;
        }

        private void OnContinueButtonClicked()
            => gameStateSwitcher.Enter<LoadLevelState>();

        private void OnSettingsButtonClicked()
        {
            windowsProvider.SetSourceWindow(WindowID.MainMenuWindow);
            windowsProvider.Open(WindowID.SettingsWindow);

            window.Close();
        }

        private void OnNewGameButtonClicked()
        {
            if (progress.HasSavedGame)
                window.SetConfirmPanelState(window.ContainerPanel, window.ConfirmPanel, true);
            else
            {
                Debug.Log("NEW GAME STARTED!");
                gameStateSwitcher.Enter<LoadLevelState>();
            }
        }

        private void OnYesButtonClicked()
        {
            PlayerPrefs.DeleteKey(ProgressSaver.ProgressKey);
            PlayerPrefs.Save();

            progressProvider.PlayerProgress.ResetProgress();
            progressSaver.SaveProgress();

            Debug.Log("PROGRESS DELETED!");
            Debug.Log("NEW GAME STARTED!");

            gameStateSwitcher.Enter<LoadLevelState>();
        }

        private void OnNoButtonClicked()
            => window.SetConfirmPanelState(window.ContainerPanel, window.ConfirmPanel, false);

        private void OnQuitGameButtonClicked()
        {
            OnCleanUped();
            Application.Quit();
        }
    }
}
