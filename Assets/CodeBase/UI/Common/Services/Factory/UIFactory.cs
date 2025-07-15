using CodeBase.Configs;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.UI.Services
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootGameObjectName = "UIRoot";

        private DIContainer container;
        private IAssetProvider assetProvider;
        private IConfigsProvider configProvider;
        private IProgressProvider progressProvider;

        public UIFactory(
            DIContainer container,
            IAssetProvider assetProvider,
            IConfigsProvider configProvider,
            IProgressProvider progressProvider)
        {
            this.container = container;
            this.assetProvider = assetProvider;
            this.configProvider = configProvider;
            this.progressProvider = progressProvider;
        }

        public Transform UIRoot { get; set; }

        public async Task WarmUp()
        {
            await assetProvider.Load<GameObject>(configProvider.GetWindowConfig(WindowID.MainMenuWindow).PrefabReference);
            await assetProvider.Load<GameObject>(configProvider.GetWindowConfig(WindowID.ShopWindow).PrefabReference);
            await assetProvider.Load<GameObject>(configProvider.GetWindowConfig(WindowID.VictoryWindow).PrefabReference);
            await assetProvider.Load<GameObject>(configProvider.GetWindowConfig(WindowID.LoseWindow).PrefabReference);
        }

        public async Task<LevelResultPresenter> CreateLevelResultWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<LevelResultWindow, LevelResultPresenter>(config);
        }

        public async Task<MainMenuPresenter> CreateMainMenuWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<MainMenuWindow, MainMenuPresenter>(config);
        }

        public async Task<ShopPresenter> CreateShopWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<ShopWindow, ShopPresenter>(config);
        }

        public async Task<ShopItem> CreateShopItemAsync()
        {
            GameObject prefab = await assetProvider.Load<GameObject>(AssetAddress.ShopItemPath);
            if (prefab == null) return null;

            GameObject shopItemGameObject = container.Instantiate(prefab);
            if (shopItemGameObject == null) return null;

            RectTransform rectTransform = shopItemGameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = prefab.GetComponent<RectTransform>().sizeDelta;
            }

            var shopItem = shopItemGameObject.GetComponent<ShopItem>();
            if (shopItem == null) return null;

            return shopItem;
        }

        public void CreateUIRoot()
        {
            UIRoot = new GameObject(UIRootGameObjectName).transform;
        }

        private async Task<TPresenter> CreateWindowAsync<TWindow, TPresenter>(WindowConfig config)
            where TWindow : WindowBase
            where TPresenter : WindowPresenterBase<TWindow>
        {
            GameObject prefab = await assetProvider.Load<GameObject>(config.PrefabReference);

            TWindow window = container.Instantiate(prefab).GetComponent<TWindow>();
            window.transform.SetParent(UIRoot);
            window.SetTitle(config.Title);

            TPresenter presenter = container.CreateAndInject<TPresenter>();
            presenter.SetWindow(window);

            return presenter;
        }
    }
}
