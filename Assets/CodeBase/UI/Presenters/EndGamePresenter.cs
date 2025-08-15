using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.UI;

namespace CodeBase.GamePlay.UI
{
    public class EndGamePresenter : WindowPresenterBase<EndGameWindow>
    {
        private EndGameWindow window;

        private IGameStateSwitcher gameStateSwitcher;

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
        private void OnMenuButtonClicked()
        {
            gameStateSwitcher.Enter<LoadMainMenuState>();
        }
    }
}
