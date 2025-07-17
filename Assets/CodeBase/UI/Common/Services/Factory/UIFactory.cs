using CodeBase.Configs;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
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

        public UIFactory(
            DIContainer container,
            IAssetProvider assetProvider,
            IConfigsProvider configProvider)
        {
            this.container = container;
            this.assetProvider = assetProvider;
            this.configProvider = configProvider;
        }

        public Transform UIRoot { get; set; }

        public async Task WarmUp()
        {
            var windowConfigs = new[]
            {
                WindowID.MainMenuWindow,
                WindowID.ShopWindow,
                WindowID.VictoryWindow,
                WindowID.LoseWindow,
                WindowID.ShrineWindow,
                WindowID.UpgradeStatsWindow
            };

            foreach (var windowConfig in windowConfigs)
                await assetProvider.Load<GameObject>(configProvider.GetWindowConfig(windowConfig).PrefabReference);
        }

        // Root
        public void CreateUIRoot() =>
            UIRoot = new GameObject(UIRootGameObjectName).transform;

        // Main Menu
        public async Task<MainMenuPresenter> CreateMainMenuWindowAsync(WindowConfig config) =>
            await CreateWindowAsync<MainMenuWindow, MainMenuPresenter>(config);

        // Level Result
        public async Task<LevelResultPresenter> CreateLevelResultWindowAsync(WindowConfig config) =>
            await CreateWindowAsync<LevelResultWindow, LevelResultPresenter>(config);

        // Shop
        public async Task<ShopPresenter> CreateShopWindowAsync(WindowConfig config) =>
            await CreateWindowAsync<ShopWindow, ShopPresenter>(config);
        public async Task<ShopItem> CreateShopItemAsync() =>
            await CreateUIItemAsync<ShopItem>(AssetAddress.ShopItemPath);

        // Shrine
        public async Task<ShrinePresenter> CreateShrineWindowAsync(WindowConfig config) =>
            await CreateWindowAsync<ShrineWindow, ShrinePresenter>(config);
        public async Task<ShrineItem> CreateShrineItemAsync() =>
            await CreateUIItemAsync<ShrineItem>(AssetAddress.ShrineItemPath);

        // Upgrade Stats
        public async Task<UpgradeStatsPresenter> CreateUpgradeStatsWindowAsync(WindowConfig config) =>
            await CreateWindowAsync<UpgradeStatsWindow, UpgradeStatsPresenter>(config);
        public async Task<UpgradeStatsItem> CreateUpgradeStatsItemAsync() =>
            await CreateUIItemAsync<UpgradeStatsItem>(AssetAddress.UpgradeStatItemPath);

        // Private
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

        private async Task<T> CreateUIItemAsync<T>(string assetPath) where T : MonoBehaviour
        {
            GameObject prefab = await assetProvider.Load<GameObject>(assetPath);
            if (prefab == null) return null;
            GameObject itemObject = container.Instantiate(prefab);
            if (itemObject == null) return null;
            SetupRectTransform(itemObject,prefab);
            return itemObject.GetComponent<T>();
        }

        private void SetupRectTransform(GameObject target, GameObject source)
        {
            var targetTransform = target.GetComponent<RectTransform>();
            var sourceTransform = source.GetComponent<RectTransform>();
            if (targetTransform != null && sourceTransform != null)
            {
                targetTransform.localScale = Vector3.one;
                targetTransform.anchoredPosition = Vector2.zero;
                targetTransform.sizeDelta = sourceTransform.sizeDelta;
            }
        }
    }
}
