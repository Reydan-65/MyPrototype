using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Infrastructure.Services.SettingsProvider;
using CodeBase.Infrastructure.Services.SettingsSaver;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class MainMenuPresenter : WindowPresenterBase<MainMenuWindow>
    {
        private MainMenuWindow window;
        private PlayerProgress progress;
        
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

            if (progress != null)
            {
                window.SetContinueButtonState(progress.HasSavedGame);
                this.window.SetDifficultyInfoText((progress.DifficultyLevel + 1).ToString());
            }

            UIClickSound[] clickSounds = window.GetComponentsInChildren<UIClickSound>();
            if (clickSounds != null && clickSounds.Length > 0)
                foreach (UIClickSound clickSound in clickSounds)
                    clickSound.SetWindow(window);

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
            progressProvider.PlayerProgress.ResetProgress();
            window.SetContinueButtonState(progress.HasSavedGame);
            window.SetDifficultyInfoText((progressProvider.PlayerProgress.DifficultyLevel + 1).ToString());
            progressSaver.SaveProgress();

            Debug.Log("PROGRESS DELETED!");
            Debug.Log("SETTINGS RESET!");
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
