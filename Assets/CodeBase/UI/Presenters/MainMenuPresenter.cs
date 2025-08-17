using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Infrastructure.Services.SettingsProvider;
using CodeBase.Infrastructure.Services.SettingsSaver;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class MainMenuPresenter : WindowPresenterBase<MainMenuWindow>
    {
        private PlayerProgress progress;
        private MainMenuWindow window;
        public MainMenuWindow Window => window;

        private IGameStateSwitcher gameStateSwitcher;
        private IProgressProvider progressProvider;
        private IWindowsProvider windowsProvider;
        private IProgressSaver progressSaver;
        private ISettingsProvider settingsProvider;
        private ISettingsSaver settingsSaver;

        public MainMenuPresenter(
            IGameStateSwitcher gameStateSwitcher,
            IProgressProvider progressProvider,
            IWindowsProvider windowsProvider,
            IProgressSaver progressSaver,
            ISettingsProvider settingsProvider,
            ISettingsSaver settingsSaver)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressProvider = progressProvider;
            this.windowsProvider = windowsProvider;
            this.progressSaver = progressSaver;
            this.settingsProvider = settingsProvider;
            this.settingsSaver = settingsSaver;
        }

        public override void SetWindow(MainMenuWindow window)
        {
            this.window = window;

            this.window.ContinueButtonClicked += OnContinueButtonClicked;
            this.window.NewGameButtonClicked += OnNewGameButtonClicked;
            this.window.SettingsButtonClicked += OnSettingsButtonClicked;
            this.window.QuitGameButtonClicked += OnQuitGameButtonClicked;
            this.window.ResetGameButtonClicked += OnResetButtonClicked;
            this.window.YesButtonClicked += OnYesButtonClicked;
            this.window.NoButtonClicked += OnNoButtonClicked;
            this.window.CleanUped += OnCleanUped;

            progress = progressSaver.GetProgress();

            this.window.SetContinueButtonState(progress.HasSavedGame);
            this.window.SetDifficultyInfoText((progress.DifficultyLevel + 1).ToString());
            this.window.ConfirmPanel.SetActive(false);
        }

        private void OnCleanUped()
        {
            window.ContinueButtonClicked -= OnContinueButtonClicked;
            window.NewGameButtonClicked -= OnNewGameButtonClicked;
            window.SettingsButtonClicked -= OnSettingsButtonClicked;
            window.QuitGameButtonClicked -= OnQuitGameButtonClicked;
            window.ResetGameButtonClicked -= OnResetButtonClicked;
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

        private void OnResetButtonClicked()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            settingsProvider.Settings.ResetSettings();
            settingsSaver.SaveSettings(settingsProvider.Settings);

            progressSaver.ResetProgress();
            progress.HasSavedGame = false;

            window.SetDifficultyInfoText((progressProvider.PlayerProgress.DifficultyLevel + 1).ToString());
            window.SetContinueButtonState(progress.HasSavedGame);

            Debug.Log("PROGRESS DELETED!");
            Debug.Log("SETTINGS RESET!");
        }

        private void OnNewGameButtonClicked()
        {
            if (progress.HasSavedGame)
                window.SetConfirmPanelState(window.ContainerPanel, window.ConfirmPanel, true);
            else
            {
                progressProvider.PlayerProgress.HasSavedGame = true;
                progressSaver.SaveProgress();

                Debug.Log("NEW GAME STARTED!");

                gameStateSwitcher.Enter<LoadLevelState>();
            }
        }

        private void OnYesButtonClicked()
        {
            PlayerPrefs.DeleteKey(ProgressSaver.ProgressKey);
            PlayerPrefs.Save();

            progressProvider.PlayerProgress.ResetProgress();
            progressProvider.PlayerProgress.HasSavedGame = true;
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
