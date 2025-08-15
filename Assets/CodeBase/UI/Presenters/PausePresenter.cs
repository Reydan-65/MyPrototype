using CodeBase.Data;
using CodeBase.GamePlay.Prototype;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Services.EntityActivityController;
using TMPro;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class PausePresenter : WindowPresenterBase<PauseWindow>
    {
        private PauseWindow window;
        private PlayerProgress progress;

        private IWindowsProvider windowsProvider;
        private IEntityActivityController entityActivityController;
        private IGameStateSwitcher gameStateSwitcher;
        private IUIFactory uiFactory;
        private IProgressSaver progressSaver;
        private IGameFactory gameFactory;

        public PausePresenter(
            IWindowsProvider windowsProvider,
            IEntityActivityController entityActivityController,
            IGameStateSwitcher gameStateSwitcher,
            IUIFactory uiFactory,
            IProgressSaver progressSaver,
            IGameFactory gameFactory)
        {
            this.windowsProvider = windowsProvider;
            this.entityActivityController = entityActivityController;
            this.gameStateSwitcher = gameStateSwitcher;
            this.uiFactory = uiFactory;
            this.progressSaver = progressSaver;
            this.gameFactory = gameFactory;
        }

        public override void SetWindow(PauseWindow window)
        {
            this.window = window;

            window.ContinueButtonClicked += OnContinue;
            window.SettingsButtonClicked += OnSettingsButtonClicked;
            window.MainMenuButtonClicked += OnMainMenuButtonClicked;
            window.QuitGameButtonClicked += OnQuitGameButtonClicked;

            window.CleanUped += OnCleanUp;

            progress = progressSaver.GetProgress();

            if (progress != null)
                this.window.SetDifficultyInfoText((progress.DifficultyLevel + 1).ToString());

            entityActivityController?.SetEntitiesActive(false);
        }

        private void OnContinue()
        {
            ContinueGame();
        }

        public void ContinueGame()
        {
            window.Close();

            PrototypeInput prototypeInput = gameFactory.PrototypeObject.GetComponent<PrototypeInput>();

            if (prototypeInput != null)
                prototypeInput.IsPaused = false;

            entityActivityController?.SetEntitiesActive(true);

            OnCleanUp();
        }

        private void OnSettingsButtonClicked()
        {
            window.Close();

            windowsProvider.SetSourceWindow(WindowID.PauseWindow);
            windowsProvider.Open(WindowID.SettingsWindow);

            window.Close();
        }

        private void OnMainMenuButtonClicked()
        {
            OnCleanUp();
            gameStateSwitcher.Enter<LoadMainMenuState>();
        }

        private void OnQuitGameButtonClicked()
        {
            Application.Quit();
        }

        public PauseWindow GetWindow() => window;

        private void OnCleanUp()
        {
            window.ContinueButtonClicked -= OnContinue;
            window.SettingsButtonClicked -= OnSettingsButtonClicked;
            window.MainMenuButtonClicked -= OnMainMenuButtonClicked;
            window.QuitGameButtonClicked -= OnQuitGameButtonClicked;

            window.CleanUped -= OnCleanUp;
        }
    }
}
