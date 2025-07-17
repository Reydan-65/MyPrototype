using CodeBase.Data;
using CodeBase.GamePlay.Hero;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsPresenter : WindowPresenterBase<UpgradeStatsWindow>
    {
        private IWindowsProvider windowsProvider;
        private IProgressSaver progressSaver;
        private IProgressProvider progressProvider;
        private IGameFactory gameFactory;

        private UpgradeStatsWindow window;

        private bool isClosed;
        private HeroStats currentStats;
        private HeroStats newStats;
        public HeroStats NewStats => newStats;

        public UpgradeStatsPresenter(
            IWindowsProvider windowsProvider,
            IProgressSaver progressSaver,
            IProgressProvider progressProvider,
            IGameFactory gameFactory)
        {
            this.windowsProvider = windowsProvider;
            this.progressSaver = progressSaver;
            this.progressProvider = progressProvider;
            this.gameFactory = gameFactory;
        }

        public override void SetWindow(UpgradeStatsWindow window)
        {
            this.window = window;

            currentStats = progressProvider.PlayerProgress.HeroStats;
            newStats = new HeroStats();
            newStats.CopyFrom(currentStats);

            window.InitializeContainer(this);

            window.AcceptButtonClicked += OnAcceptButtonClicked;
            window.CancelButtonClicked += OnCancelButtonClicked;
            window.Closed += OnClosed;
            window.CleanUped += OnCleanUp;
        }

        private void OnAcceptButtonClicked()
        {
            if (currentStats == null || newStats == null) return;

            if (!AreStatsEqual(currentStats, newStats))
            {
                currentStats.CopyFrom(newStats);
                currentStats.IsChanged();
            }

            progressSaver.SaveProgress();
            progressProvider.PlayerProgress.HeroStats.IsChanged();

            gameFactory.HeroObject.GetComponent<HeroHealth>().Initialize(currentStats.MaxHitPoints);
            gameFactory.HeroObject.GetComponent<HeroTurret>().Initialize(currentStats.ShootingRate);
            gameFactory.HeroObject.GetComponent<HeroMovement>().Initialize(currentStats.MovementSpeed, currentStats.DashRange);

            OnClosed();
        }

        private void OnCancelButtonClicked()
        {
            if (currentStats == null || newStats == null) return;

            if (!AreStatsEqual(currentStats, newStats))
            {
                newStats.CopyFrom(currentStats);
                window.UpdateStatsDisplay();
            }
            OnClosed();
        }

        private bool AreStatsEqual(HeroStats a, HeroStats b)
        {
            return a.MaxHitPoints == b.MaxHitPoints &&
                   a.ShootingRate == b.ShootingRate &&
                   a.DashRange == b.DashRange &&
                   a.MovementSpeed == b.MovementSpeed;
        }

        private void OnCleanUp()
        {
            window.AcceptButtonClicked -= OnAcceptButtonClicked;
            window.CancelButtonClicked -= OnCancelButtonClicked;

            window.Closed -= OnClosed;
            window.CleanUped -= OnCleanUp;
        }

        private void OnClosed()
        {
            if(isClosed) return;
            isClosed = true;
            window.Close();
            windowsProvider.Open(WindowID.ShrineWindow);
        }
    }
}
