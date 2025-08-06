using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Services.EntityActivityController;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class PausePresenter : WindowPresenterBase<PauseWindow>
    {
        private PauseWindow window;

        private IWindowsProvider windowsProvider;
        private IEntityActivityController entityActivityController;
        private IGameStateSwitcher gameStateSwitcher;
        private IUIFactory uiFactory;
        public PausePresenter(
            IWindowsProvider windowsProvider,
            IEntityActivityController entityActivityController,
            IGameStateSwitcher gameStateSwitcher,
            IUIFactory uiFactory)
        {
            this.windowsProvider = windowsProvider;
            this.entityActivityController = entityActivityController;
            this.gameStateSwitcher = gameStateSwitcher;
            this.uiFactory = uiFactory;
        }

        public override void SetWindow(PauseWindow window)
        {
            this.window = window;

            window.ContinueButtonClicked += OnContinue;
            window.SettingsButtonClicked += OnSettingsButtonClicked;
            window.MainMenuButtonClicked += OnMainMenuButtonClicked;
            window.QuitGameButtonClicked += OnQuitGameButtonClicked;

            window.CleanUped += OnCleanUp;

            entityActivityController?.SetEntitiesActive(false);
        }

        private void OnContinue()
        {
            ContinueGame();
        }

        public void ContinueGame()
        {
            window.Close();

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
