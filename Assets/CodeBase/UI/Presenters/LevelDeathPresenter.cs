using CodeBase.Infrastructure.Services.GameStateMachine;

namespace CodeBase.GamePlay.UI
{
    public class LevelDeathPresenter : WindowPresenterBase<LevelDeathWindow>
    {
        private IGameStateSwitcher gameStateSwitcher;
        private LevelDeathWindow window;

        public LevelDeathPresenter(
            IGameStateSwitcher gameStateSwitcher)
        {
            this.gameStateSwitcher = gameStateSwitcher;
        }

        public override void SetWindow(LevelDeathWindow window)
        {
            window.CleanUped += OnCleanUped;

            this.window = window;
        }

        private void OnCleanUped()
        {
            window.CleanUped -= OnCleanUped;
        }
    }
}
