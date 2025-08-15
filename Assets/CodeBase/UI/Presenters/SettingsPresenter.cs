using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.SettingsProvider;

namespace CodeBase.GamePlay.UI
{
    public class SettingsPresenter : WindowPresenterBase<SettingsWindow>
    {
        private WindowID sourceWindow;
        public WindowID SourceWindow => sourceWindow;

        private SettingsWindow window;

        private IWindowsProvider windowsProvider;
        private ISettingsProvider settingsProvider;

        public SettingsPresenter(IWindowsProvider windowsProvider, ISettingsProvider settingsProvider)
        {
            this.windowsProvider = windowsProvider;
            this.settingsProvider = settingsProvider;
        }

        public override void SetWindow(SettingsWindow window)
        {
            this.window = window;
            this.window.InitializeContainer(this);

            Subscribes();
            UpdateButtonsState();
        }

        public void SetSourceWindow(WindowID sourceWindow) => this.sourceWindow = sourceWindow;

        private void OnDefaultButtonClicked()
        {
            window.Container.ResetToDefaults();
            window.DefaultButton.interactable = false;
            window.AcceptButton.interactable = true;
        }

        private void OnAcceptButtonClicked()
        {
            window.Container.ApplyChanges();

            UpdateButtonsState();
        }

        private void OnAdvancedCloseButtonClicked()
        {
            if (!Settings.SettingsAreEqual(window.Container.NewSettings, settingsProvider.Settings))
                window.SetConfirmPanelState(window.MainBottomPanel, window.ConfirmBottomPanel, true);
            else
                Close();
        }

        private void OnYesButtonClicked()
        {
            window.Container.RevertChanges();
            Close();
        }

        private void OnNoButtonClicked() 
            => window.SetConfirmPanelState(window.MainBottomPanel, window.ConfirmBottomPanel, false);

        public SettingsWindow GetWindow() => window;
        private void OnSettingsChanged() => UpdateButtonsState();
        private void OnCleanUp() => Unsubscribes();

        private void UpdateButtonsState()
        {
            window.AcceptButton.interactable =
                !Settings.SettingsAreEqual(window.Container.NewSettings, settingsProvider.Settings);
            window.DefaultButton.interactable =
                !Settings.SettingsAreEqual(window.Container.NewSettings, Settings.GetDefaultSettings());
        }

        private void Close()
        {
            if (windowsProvider.SourceWindowID == WindowID.MainMenuWindow)
                windowsProvider.Open(WindowID.MainMenuWindow);

            if (windowsProvider.SourceWindowID == WindowID.PauseWindow)
                windowsProvider.Open(WindowID.PauseWindow);

            window.Close();
        }

        private void Subscribes()
        {
            if (window == null) return;

            window.AcceptButtonClicked += OnAcceptButtonClicked;
            window.DefaultButtonClicked += OnDefaultButtonClicked;
            window.AdvancedCloseButtonClicked += OnAdvancedCloseButtonClicked;
            window.YesButtonClicked += OnYesButtonClicked;
            window.NoButtonClicked += OnNoButtonClicked;
            window.CleanUped += OnCleanUp;

            if (window.Container == null) return;

            window.Container.NewSettings.Changed += OnSettingsChanged;
        }
        private void Unsubscribes()
        {
            if (window == null) return;

            window.AcceptButtonClicked -= OnAcceptButtonClicked;
            window.DefaultButtonClicked -= OnDefaultButtonClicked;
            window.AdvancedCloseButtonClicked -= OnAdvancedCloseButtonClicked;
            window.YesButtonClicked -= OnYesButtonClicked;
            window.NoButtonClicked -= OnNoButtonClicked;
            window.CleanUped -= OnCleanUp;

            if (window.Container == null) return;

            window.Container.NewSettings.Changed -= OnSettingsChanged;
        }
    }
}
