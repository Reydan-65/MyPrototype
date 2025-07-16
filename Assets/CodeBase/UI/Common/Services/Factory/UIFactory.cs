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

        public async Task<MainMenuPresenter> CreateMainMenuWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<MainMenuWindow, MainMenuPresenter>(config);
        }

        public async Task<LevelResultPresenter> CreateLevelResultWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<LevelResultWindow, LevelResultPresenter>(config);
        }

        #region Shop
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
        #endregion

        #region Shrine
        public async Task<ShrinePresenter> CreateShrineWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<ShrineWindow, ShrinePresenter>(config);
        }

        public async Task<ShrineItem> CreateShrineItemAsync()
        {
            GameObject prefab = await assetProvider.Load<GameObject>(AssetAddress.ShrineItemPath);
            if (prefab == null) return null;

            GameObject shrineElementGameObject = container.Instantiate(prefab);
            if (shrineElementGameObject == null) return null;

            RectTransform rectTransform = shrineElementGameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = prefab.GetComponent<RectTransform>().sizeDelta;
            }

            var shrineElement = shrineElementGameObject.GetComponent<ShrineItem>();
            if (shrineElement == null) return null;

            return shrineElement;
        }
        #endregion

        #region UpgradeStats
        public async Task<UpgradeStatsPresenter> CreateUpgradeStatsWindowAsync(WindowConfig config)
        {
            return await CreateWindowAsync<UpgradeStatsWindow, UpgradeStatsPresenter>(config);
        }

        public async Task<UpgradeStatsItem> CreateUpgradeStatsItemAsync()
        {
            GameObject prefab = await assetProvider.Load<GameObject>(AssetAddress.UpgradeStatItemPath);
            if (prefab == null) return null;

            GameObject upgradeStatsItemGameObject = container.Instantiate(prefab);
            if (upgradeStatsItemGameObject == null) return null;

            RectTransform rectTransform = upgradeStatsItemGameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = prefab.GetComponent<RectTransform>().sizeDelta;
            }

            var upgradeStatsItem = upgradeStatsItemGameObject.GetComponent<UpgradeStatsItem>();
            if (upgradeStatsItem == null) return null;

            return upgradeStatsItem;
        }
        #endregion

        #region Root
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
        #endregion
    }
}
