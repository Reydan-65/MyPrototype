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
            this.window.AdvancedCloseButtonClicked += OnAdvancedCloseButtonClicked;
            this.window.YesButtonClicked += OnYesButtonClicked;
            this.window.NoButtonClicked += OnNoButtonClicked;
            this.window.Closed += OnClosed;
            this.window.CleanUped += OnCleanUp;
            this.window.Container.PendingStats.Changed += UpdateButtonsState;
            this.window.Container.PendingProjectileStats.Changed += UpdateButtonsState;
            this.window.AcceptButton.interactable = false;

            OnNoButtonClicked();
        }

        private void OnAcceptButtonClicked()
        {
            window.Container.ApplyAllUpgrades();
            SaveAndApply();
            window.AcceptButton.interactable = false;
        }

        private void OnYesButtonClicked()
        {
            window.Container.RevertChanges();
            OnClosed();
        }

        private void OnNoButtonClicked()
        {
            window.SetConfirmPanelState(window.MainBottomPanel, window.ConfirmBottomPanel, false);
        }

        private void UpdateButtonsState()
            => window.AcceptButton.interactable = WasChanges();


        private void SaveAndApply()
        {
            gameFactory.PrototypeObject.GetComponent<PrototypeHealth>().Initialize(currentStats.Health.Value);
            gameFactory.PrototypeObject.GetComponent<PrototypeEnergy>().Initialize(currentStats.Energy.Value);
            gameFactory.PrototypeObject.GetComponent<PrototypeTurret>().Initialize(currentStats.ShootingRate.Value);
            gameFactory.PrototypeObject.GetComponent<PrototypeMovement>().Initialize(currentStats.MovementSpeed.Value, currentStats.DashRange.Value);

            progressProvider.PlayerProgress.PrototypeStats.IsChanged();
            progressProvider.PlayerProgress.ProjectileStats.IsChanged();

            progressSaver.SaveProgress();
        }

        private void OnAdvancedCloseButtonClicked()
        {
            if (WasChanges())
                window.SetConfirmPanelState(window.MainBottomPanel, window.ConfirmBottomPanel, true);
            else
                OnClosed();
        }

        private bool WasChanges()
        {
            return !PrototypeStats.StatsAreEqual(window.Container.PendingStats, progressProvider.PlayerProgress.PrototypeStats) ||
                   !ProjectileStats.StatsAreEqual(window.Container.PendingProjectileStats, progressProvider.PlayerProgress.ProjectileStats);
        }

        private void OnCleanUp()
        {
            window.AcceptButtonClicked -= OnAcceptButtonClicked;
            window.AdvancedCloseButtonClicked -= OnAdvancedCloseButtonClicked;
            window.YesButtonClicked -= OnYesButtonClicked;
            window.NoButtonClicked -= OnNoButtonClicked;
            window.Closed -= OnClosed;
            window.CleanUped -= OnCleanUp;
            window.Container.PendingStats.Changed -= UpdateButtonsState;
            window.Container.PendingProjectileStats.Changed -= UpdateButtonsState;
        }

        private void OnClosed()
        {
            if (isClosed) return;
            isClosed = true;
            window.Close();
            windowsProvider.Open(WindowID.ShrineWindow);
        }
    }
}
