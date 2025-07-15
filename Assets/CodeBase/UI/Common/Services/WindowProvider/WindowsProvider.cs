using CodeBase.Configs;
using CodeBase.Infrastructure.Services.ConfigProvider;

namespace CodeBase.GamePlay.UI.Services
{
    public class WindowsProvider : IWindowsProvider
    {
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

            if (id == WindowID.VictoryWindow || id == WindowID.LoseWindow)
                factory.CreateLevelResultWindowAsync(config);

            if (id == WindowID.MainMenuWindow)
                factory.CreateMainMenuWindowAsync(config);

            if (id == WindowID.ShopWindow)
                factory.CreateShopWindowAsync(config);
        }
    }
}
