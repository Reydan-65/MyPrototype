using CodeBase.Configs;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.UI.Elements;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.UI.Services
{
    public interface IUIFactory : IService
    {
        event UnityAction HUDWindowCreated;

        Task WarmUp();

        Transform UIRoot { get; set; }
        HUDPresenter HUDPresenter { get; set; }
        HUDWindow HUDWindow { get; set; }
        ShrinePresenter ShrinePresenter { get; set; }
        ShrineWindow ShrineWindow { get; set; }
        PausePresenter PausePresenter { get; set; }
        PauseWindow PauseWindow { get; set; }
        SettingsPresenter SettingsPresenter { get; set; }
        SettingsWindow SettingsWindow { get; set; }
        LevelResultPresenter LevelResultPresenter { get; set; }
        LevelResultWindow LevelResultWindow { get; set; }

        void CreateUIRoot();

        Task<MainMenuPresenter> CreateMainMenuWindowAsync(WindowConfig config);
        Task<LevelResultPresenter> CreateLevelResultWindowAsync(WindowConfig config);
        Task<HUDPresenter> CreateHUDWindowAsync(WindowConfig config);
        Task<PausePresenter> CreatePauseWindowAsync(WindowConfig config);

        Task<SettingsPresenter> CreateSettingsWindowAsync(WindowConfig config);
        Task<SettingsItem> CreateSettingsItemAsync();

        Task<ShopPresenter> CreateShopWindowAsync(WindowConfig config);
        Task<ShopItem> CreateShopItemAsync();

        Task<ShrinePresenter> CreateShrineWindowAsync(WindowConfig config);
        Task<ShrineItem> CreateShrineItemAsync();

        Task<UpgradeStatsPresenter> CreateUpgradeStatsWindowAsync(WindowConfig config);
        Task<UpgradeStatsItem> CreateUpgradeStatsItemAsync();
    }
}