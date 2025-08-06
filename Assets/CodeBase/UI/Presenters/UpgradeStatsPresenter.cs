using CodeBase.Data;
using CodeBase.GamePlay.Prototype;
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
        private PrototypeStats currentStats;
        private PrototypeStats newStats;
        public PrototypeStats NewStats => newStats;

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

            currentStats = progressProvider.PlayerProgress.PrototypeStats;

            newStats = new PrototypeStats();
            newStats.CopyFrom(currentStats);

            this.window.InitializeContainer(this);

            this.window.AcceptButtonClicked += OnAcceptButtonClicked;
            this.window.CancelButtonClicked += OnCancelButtonClicked;
            this.window.SetCancelButtonText("CLOSE");
            this.window.Closed += OnClosed;
            this.window.CleanUped += OnCleanUp;
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
            progressProvider.PlayerProgress.PrototypeStats.IsChanged();

            gameFactory.PrototypeObject.GetComponent<PrototypeHealth>().Initialize(currentStats.MaxHitPoints);
            gameFactory.PrototypeObject.GetComponent<PrototypeEnergy>().Initialize(currentStats.MaxEnergy);
            gameFactory.PrototypeObject.GetComponent<PrototypeTurret>().Initialize(currentStats.ShootingRate);
            gameFactory.PrototypeObject.GetComponent<PrototypeMovement>().Initialize(currentStats.MovementSpeed, currentStats.DashRange);

            OnClosed();
        }

        private void OnCancelButtonClicked()
        {
            if (currentStats == null || newStats == null) return;

            if (!AreStatsEqual(currentStats, newStats))
            {
                newStats.CopyFrom(currentStats);
                window.UpdateStatsDisplay();
                window.SetCancelButtonText("CANCEL");
            }
            OnClosed();
        }

        private bool AreStatsEqual(PrototypeStats a, PrototypeStats b)
        {
            return a.MaxHitPoints == b.MaxHitPoints &&
                   a.MaxEnergy == b.MaxEnergy &&
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
