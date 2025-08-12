using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.GamePlay.UI
{
    public class EndGamePresenter : WindowPresenterBase<EndGameWindow>
    {
        private EndGameWindow window;

        private IProgressProvider progressProvider;
        private IProgressSaver progressSaver;
        private IGameStateSwitcher gameStateSwitcher;

        public EndGamePresenter(
            IGameStateSwitcher gameStateSwitcher,
            IProgressProvider progressProvider,
            IProgressSaver progressSaver)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressProvider = progressProvider;
            this.progressSaver = progressSaver;

        }

        public override void SetWindow(EndGameWindow window)
        {
            this.window = window;
            this.window.MenuButtonClicked += OnMenuButtonClicked;
            this.window.CleanUped += OnCleanUped;
        }

        private void OnCleanUped()
        {
            window.MenuButtonClicked -= OnMenuButtonClicked;
            window.CleanUped -= OnCleanUped;
        }

        public EndGameWindow GetWindow() => window;
        private void OnMenuButtonClicked()
        {
            gameStateSwitcher.Enter<LoadMainMenuState>();
        }
    }
}
