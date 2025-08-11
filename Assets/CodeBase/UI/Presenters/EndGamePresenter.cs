//using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;

namespace CodeBase.GamePlay.UI
{
    public class EndGamePresenter : WindowPresenterBase<EndGameWindow>
    {
        private IGameStateSwitcher gameStateSwitcher;
        private EndGameWindow window;

        public EndGamePresenter(IGameStateSwitcher gameStateSwitcher)
            => this.gameStateSwitcher = gameStateSwitcher;

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
        private void OnMenuButtonClicked() => gameStateSwitcher.Enter<LoadMainMenuState>();
    }
}
