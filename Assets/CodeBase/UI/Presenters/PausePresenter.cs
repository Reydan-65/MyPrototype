using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.EntityActivityController;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class PausePresenter : WindowPresenterBase<PauseWindow>
    {
        private IWindowsProvider windowsProvider;
        private IEntityActivityController entityActivityController;

        private PauseWindow window;

        public PausePresenter(
            IWindowsProvider windowsProvider,
            IEntityActivityController entityActivityController)
        {
            this.windowsProvider = windowsProvider;
            this.entityActivityController = entityActivityController;
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
            Debug.Log("OPEN SETTINGS WINDOW");
        }

        private void OnMainMenuButtonClicked()
        {
            windowsProvider.Open(WindowID.MainMenuWindow);
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
