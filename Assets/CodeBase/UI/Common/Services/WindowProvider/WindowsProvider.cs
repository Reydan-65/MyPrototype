using CodeBase.Configs;
using CodeBase.Infrastructure.Services.ConfigProvider;

namespace CodeBase.GamePlay.UI.Services
{
    public class WindowsProvider : IWindowsProvider
    {
        private WindowID sourceWindowID;
        public WindowID SourceWindowID => sourceWindowID;

        private IUIFactory factory;
        private IConfigsProvider configsProvider;

        public WindowsProvider(IUIFactory factory, IConfigsProvider configsProvider)
        {
            this.factory = factory;
            this.configsProvider = configsProvider;
        }

        public void Open(WindowID id)
        {
            if (factory.UIRoot == null)
                factory.CreateUIRoot();

            WindowConfig config = configsProvider.GetWindowConfig(id);

            if (id == WindowID.EndGameWindow || id == WindowID.DeathWindow)
                factory.CreateLevelEndWindowAsync(config);

            if (id == WindowID.MainMenuWindow)
                factory.CreateMainMenuWindowAsync(config);

            if (id == WindowID.ShopWindow)
                factory.CreateShopWindowAsync(config);

            if (id == WindowID.ShrineWindow)
                factory.CreateShrineWindowAsync(config);

            if (id == WindowID.UpgradesWindow)
                factory.CreateUpgradesWindowAsync(config);

            if (id == WindowID.HUDWindow)
                factory.CreateHUDWindowAsync(config);

            if (id == WindowID.PauseWindow)
                factory.CreatePauseWindowAsync(config);

            if (id == WindowID.SettingsWindow)
            {
                factory.CreateSettingsWindowAsync(config);
            }
        }

        public void SetSourceWindow(WindowID id) => sourceWindowID = id;
    }
}
