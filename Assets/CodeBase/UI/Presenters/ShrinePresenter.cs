using CodeBase.GamePlay.Prototype;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.Factory;

namespace CodeBase.GamePlay.UI
{
    public class ShrinePresenter : WindowPresenterBase<ShrineWindow>
    {
        private ShrineWindow window;

        private IWindowsProvider windowsProvider;
        private IGameFactory gameFactory;
        
        public ShrinePresenter(IWindowsProvider windowsProvider, IGameFactory gameFactory)
        {
            this.windowsProvider = windowsProvider;
            this.gameFactory = gameFactory;
        }

        public override void SetWindow(ShrineWindow window)
        {
            this.window = window;

            window.UpgradeStatsButtonClicked += OnUpgradeStatsButtonClicked;

            gameFactory.PrototypeObject.GetComponent<PrototypeInput>().HasOpenedWindow = true;

            window.CleanUped += OnCleanUp;
            this.window.Closed += OnClosed;
        }

        private void OnCleanUp()
        {
            window.UpgradeStatsButtonClicked -= OnUpgradeStatsButtonClicked;

            window.CleanUped -= OnCleanUp;
            window.Closed -= OnClosed;
        }

        public ShrineWindow GetWindow() => window;

        private void OnUpgradeStatsButtonClicked()
        {
            OnClosed();
            windowsProvider.Open(WindowID.UpgradesWindow);
        }

        private void OnClosed()
        {
            window.Closed -= OnClosed;

            gameFactory.PrototypeObject.GetComponent<PrototypeInput>().HasOpenedWindow = false;
            window.Close();
        }
    }
}
