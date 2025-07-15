using CodeBase.GamePlay.UI.Services;

namespace CodeBase.GamePlay.UI
{
    public class ShopPresenter : WindowPresenterBase<ShopWindow>
    {
        private IWindowsProvider windowsProvider;
        private ShopWindow window;

        public ShopPresenter(IWindowsProvider windowsProvider)
        {
            this.windowsProvider = windowsProvider;
        }

        public override void SetWindow(ShopWindow window)
        {
            this.window = window;

            window.Closed += OnClosed;
            window.CleanUped += OnCleanUp;
        }

        private void OnCleanUp()
        {
            window.Closed -= OnClosed;
            window.CleanUped -= OnCleanUp;
        }

        private void OnClosed()
        {
            windowsProvider.Open(WindowID.MainMenuWindow);
        }
    }
}
