using CodeBase.GamePlay.UI.Services;

namespace CodeBase.GamePlay.UI
{
    public class ShrinePresenter : WindowPresenterBase<ShrineWindow>
    {
        private IWindowsProvider windowsProvider;
        private ShrineWindow window;

        public ShrinePresenter(IWindowsProvider windowsProvider)
        {
            this.windowsProvider = windowsProvider;
        }

        public override void SetWindow(ShrineWindow window)
        {
            this.window = window;

            window.UpgradeStatsButtonClicked += OnUpgradeStatsButtonClicked;

            window.CleanUped += OnCleanUp;
        }

        private void OnCleanUp()
        {
            window.UpgradeStatsButtonClicked -= OnUpgradeStatsButtonClicked;

            window.CleanUped -= OnCleanUp;
        }

        public ShrineWindow GetWindow() => window;

        private void OnUpgradeStatsButtonClicked()
        {
            window.Close();
            windowsProvider.Open(WindowID.UpgradeStatsWindow);
        }
    }
}
