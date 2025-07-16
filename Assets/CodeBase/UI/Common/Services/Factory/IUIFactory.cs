using CodeBase.Configs;
using CodeBase.Infrastructure.DependencyInjection;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.UI.Services
{
    public interface IUIFactory : IService
    {
        Task WarmUp();

        Transform UIRoot { get; set; }
        void CreateUIRoot();

        Task<MainMenuPresenter> CreateMainMenuWindowAsync(WindowConfig config);
        Task<LevelResultPresenter> CreateLevelResultWindowAsync(WindowConfig config);

        Task<ShopPresenter> CreateShopWindowAsync(WindowConfig config);
        Task<ShopItem> CreateShopItemAsync();

        Task<ShrinePresenter> CreateShrineWindowAsync(WindowConfig config);
        Task<ShrineItem> CreateShrineItemAsync();

        Task<UpgradeStatsPresenter> CreateUpgradeStatsWindowAsync(WindowConfig config);
        Task<UpgradeStatsItem> CreateUpgradeStatsItemAsync();
    }
}