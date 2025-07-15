using CodeBase.Configs;
using CodeBase.Infrastructure.DependencyInjection;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.UI.Services
{
    public interface IUIFactory : IService
    {
        Transform UIRoot { get; set; }
        Task WarmUp();
        Task<LevelResultPresenter> CreateLevelResultWindowAsync(WindowConfig config);
        Task<MainMenuPresenter> CreateMainMenuWindowAsync(WindowConfig config);
        Task<ShopPresenter> CreateShopWindowAsync(WindowConfig config);
        Task<ShopItem> CreateShopItemAsync();
        void CreateUIRoot();
    }
}