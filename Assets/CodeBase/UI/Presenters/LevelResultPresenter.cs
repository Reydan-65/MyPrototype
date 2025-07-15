//using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;

namespace CodeBase.GamePlay.UI
{
    public class LevelResultPresenter : WindowPresenterBase<LevelResultWindow>
    {
        private IGameStateSwitcher gameStateSwitcher;
        //private IAdsService adsService;
        private LevelResultWindow window;

        public LevelResultPresenter(
            IGameStateSwitcher gameStateSwitcher/*,
            IAdsService adsService*/)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            //this.adsService = adsService;
        }

        public override void SetWindow(LevelResultWindow window)
        {
            window.MenuButtonClicked += OnMenuButtonClicked;
            window.CleanUped += OnCleanUped;

            this.window = window;
        }

        private void OnCleanUped()
        {
            window.MenuButtonClicked -= OnMenuButtonClicked;
            window.CleanUped -= OnCleanUped;
        }

        private void OnMenuButtonClicked()
        {
            gameStateSwitcher.Enter<LoadMainMenuState>();
            //adsService.ShowInterstitial();
        }
    }
}
