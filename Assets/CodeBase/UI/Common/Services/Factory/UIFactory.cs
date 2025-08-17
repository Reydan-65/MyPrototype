using CodeBase.Configs;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.UI;
using CodeBase.UI.Elements;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.UI.Services
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootGameObjectName = "UIRoot";

        public event UnityAction HUDWindowCreated;

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
        public HUDPresenter HUDPresenter { get; set; }
        public HUDWindow HUDWindow { get; set; }
        public ShrinePresenter ShrinePresenter { get; set; }
        public ShrineWindow ShrineWindow { get; set; }
        public PausePresenter PausePresenter { get; set; }
        public PauseWindow PauseWindow { get; set; }
        public SettingsPresenter SettingsPresenter { get; set; }
        public SettingsWindow SettingsWindow { get; set; }
        public EndGamePresenter EndGamePresenter { get; set; }
        public EndGameWindow EndGameWindow { get; set; }

        public async Task WarmUp()
        {
            var windowConfigs = new[]
            {
                WindowID.MainMenuWindow,
                WindowID.EndGameWindow,
                WindowID.DeathWindow,
                WindowID.ShrineWindow,
                WindowID.UpgradesWindow,
                WindowID.HUDWindow,
                WindowID.PauseWindow,
                WindowID.SettingsWindow
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
        public async Task<EndGamePresenter> CreateLevelEndWindowAsync(WindowConfig config)
        {
            EndGamePresenter = await CreateWindowAsync<EndGameWindow, EndGamePresenter>(config);
            EndGameWindow = EndGamePresenter.GetWindow();
            return EndGamePresenter;
        }

        // HUD
        public async Task<HUDPresenter> CreateHUDWindowAsync(WindowConfig config)
        {
            HUDPresenter = await CreateWindowAsync<HUDWindow, HUDPresenter>(config);
            HUDWindow = HUDPresenter.GetWindow();
            HUDWindowCreated?.Invoke();
            return HUDPresenter;
        }

        // Pause
        public async Task<PausePresenter> CreatePauseWindowAsync(WindowConfig config)
        {
            PausePresenter = await CreateWindowAsync<PauseWindow, PausePresenter>(config);
            PauseWindow = PausePresenter.GetWindow();
            return PausePresenter;
        }

        // Shrine
        public async Task<ShrinePresenter> CreateShrineWindowAsync(WindowConfig config)
        {
            ShrinePresenter = await CreateWindowAsync<ShrineWindow, ShrinePresenter>(config);
            ShrineWindow = ShrinePresenter.GetWindow();
            return ShrinePresenter;
        }

        public async Task<ShrineItem> CreateShrineItemAsync() =>
            await CreateUIItemAsync<ShrineItem>(AssetAddress.ShrineItemPath);

        // Upgrade Stats
        public async Task<UpgradesPresenter> CreateUpgradesWindowAsync(WindowConfig config) =>
            await CreateWindowAsync<UpgradesWindow, UpgradesPresenter>(config);
        public async Task<UpgradesItem> CreateUpgradesItemAsync() =>
            await CreateUIItemAsync<UpgradesItem>(AssetAddress.UpgradesItemPath);
        public async Task<UpgradesHeaderItem> CreateUpgradesHeaderItemAsync() =>
            await CreateUIItemAsync<UpgradesHeaderItem>(AssetAddress.UpgradesHeaderItemPath);

        // Shop
        public async Task<SettingsPresenter> CreateSettingsWindowAsync(WindowConfig config)
        {
            SettingsPresenter = await CreateWindowAsync<SettingsWindow, SettingsPresenter>(config);
            SettingsWindow = SettingsPresenter.GetWindow();
            return SettingsPresenter;
        }

        public async Task<SettingsItem> CreateSettingsItemAsync()
        {
            try
            {
                var item = await CreateUIItemAsync<SettingsItem>(AssetAddress.SettingsItemPath);
                if (item == null)
                    Debug.LogError("SettingsItem prefab is null!");

                UIClickSound[] clickSounds = item.GetComponentsInChildren<UIClickSound>();
                if (clickSounds != null && clickSounds.Length > 0)
                    foreach (var clickSound in clickSounds)
                        clickSound.SetWindow(SettingsWindow);
                return item;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error creating SettingsItem: {e}");
                return null;
            }
        }

        // Private
        private async Task<TPresenter> CreateWindowAsync<TWindow, TPresenter>(WindowConfig config)
            where TWindow : WindowBase
            where TPresenter : WindowPresenterBase<TWindow>
        {
            GameObject prefab = await assetProvider.Load<GameObject>(config.PrefabReference);
            TWindow window = container.Instantiate(prefab).GetComponent<TWindow>();
            window.transform.SetParent(UIRoot);
            if (config.Title != null) window.SetTitle(config.Title);
            TPresenter presenter = container.CreateAndInject<TPresenter>();
            presenter.SetWindow(window);
            presenter.AssignWindowToClickSounds(window);
            window.SetSFXPlayer();
            return presenter;
        }

        private async Task<T> CreateUIItemAsync<T>(string assetPath) where T : MonoBehaviour
        {
            GameObject prefab = await assetProvider.Load<GameObject>(assetPath);
            if (prefab == null) return null;
            GameObject itemObject = container.Instantiate(prefab);
            if (itemObject == null) return null;
            SetupRectTransform(itemObject, prefab);
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
