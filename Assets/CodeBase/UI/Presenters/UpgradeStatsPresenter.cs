using CodeBase.GamePlay.UI.Services;
using System;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsPresenter : WindowPresenterBase<UpgradeStatsWindow>
    {
        private UpgradeStatsWindow window;

        public override void SetWindow(UpgradeStatsWindow window)
        {
            this.window = window;

            window.CleanUped += OnCleanUp;
        }

        private void OnCleanUp()
        {
            window.CleanUped -= OnCleanUp;
        }
    }
}
