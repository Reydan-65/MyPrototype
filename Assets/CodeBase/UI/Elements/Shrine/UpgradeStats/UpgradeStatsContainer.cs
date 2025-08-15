using CodeBase.Data;
using CodeBase.GamePlay.Projectile;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Sounds;
using CodeBase.UI;
using CodeBase.UI.Elements;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsContainer : MonoBehaviour
    {
        private const float HEALTHBONUS = 2;
        private const float ENERGYBONUS = 1;
        private const float FIRERATEBONUS = 0.01f;
        private const float SPEEDBONUS = 0.02f;
        private const float DASHRANGEBONUS = 0.02f;
        private const float PPROJECTILEDAMAGEBONUS = 1f;
        private const float PPROJECTILESPEEDBONUS = 0.02f;
        private const float PPROJECTILERANGEBONUS = 0.02f;

        [SerializeField] private Transform parent;
        [SerializeField] private Scrollbar scrollbar;

        private SFXPlayer sfxPlayer;
        private List<UpgradesItem> statItems = new List<UpgradesItem>();
        private ProjectileStats pendingProjectileStats;
        private UpgradesWindow window;
        private PrototypeStats pendingStats;
        private bool isInitialized;

        public List<UpgradesItem> StatItems => statItems;
        public PrototypeStats PendingStats => pendingStats;
        public ProjectileStats PendingProjectileStats => pendingProjectileStats;

        private IUIFactory uiFactory;
        private IProgressProvider progressProvider;
        private IGameFactory gameFactory;
        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(
            IUIFactory uiFactory,
            IProgressProvider progressProvider,
            IGameFactory gameFactory,
            IAssetProvider assetProvider)
        {
            this.uiFactory = uiFactory;
            this.progressProvider = progressProvider;
            this.gameFactory = gameFactory;
            this.assetProvider = assetProvider;
        }

        public void Initialize(UpgradesWindow window)
        {
            if (isInitialized) return;

            this.window = window;
            sfxPlayer = gameFactory.AudioPlayer.GetComponent<SFXPlayer>();
            isInitialized = true;

            pendingStats = new PrototypeStats();
            pendingStats.CopyFrom(progressProvider.PlayerProgress.PrototypeStats);

            pendingProjectileStats = new ProjectileStats();
            pendingProjectileStats.CopyFrom(progressProvider.PlayerProgress.ProjectileStats);

            if (progressProvider?.PlayerProgress?.PrototypeStats != null)
            {
                progressProvider.PlayerProgress.PrototypeStats.Changed -= UpdateAvailableElements;
                progressProvider.PlayerProgress.PrototypeStats.Changed += UpdateAvailableElements;
            }

            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
            {
                progressProvider.PlayerProgress.PrototypeInventoryData.CoinValueChanged -= OnCoinValueChanged;
                progressProvider.PlayerProgress.PrototypeInventoryData.CoinValueChanged += OnCoinValueChanged;
            }

            UpdateAvailableElements();
            UpdateScrollbarVisibility();
        }

        private void UpdateScrollbarVisibility()
        {
            if (scrollbar != null)
            {
                float totalHeight = 0f;
                foreach (var item in statItems)
                {
                    if (item.TryGetComponent<RectTransform>(out var rt))
                        totalHeight += rt.rect.height;
                }
            }
        }

        public void UpdateAvailableElements()
        {
            if (statItems.Count == 0)
                CreateStatElements();
            else
                UpdateExistingElements();
            UpdateScrollbarVisibility();
        }

        public void ApplyAllUpgrades()
        {
            foreach (var item in statItems)
            {
                item.ApplyUpgrades();
                UpdateStatLevelInProgress(item.StatName, item.CurrentLevel);
            }
            progressProvider.PlayerProgress.PrototypeStats.CopyFrom(pendingStats);
            progressProvider.PlayerProgress.ProjectileStats.CopyFrom(pendingProjectileStats);
            progressProvider.PlayerProgress.HasSavedGame = true;
        }

        public void ResetAllUpgrades()
        {
            foreach (var item in statItems)
                item.ResetUpgrades();
            pendingStats.CopyFrom(progressProvider.PlayerProgress.PrototypeStats);
        }

        private void ClearContainer()
        {
            foreach (var item in statItems)
                Destroy(item.gameObject);
            statItems.Clear();
        }

        private async void CreateStatElements()
        {
            await CreateHeaderElementAsync("PROTOTYPE");
            await CreateStatElementAsync("HEALTH", pendingStats.Health.Value, HEALTHBONUS, 1, pendingStats.Health.Level);
            await CreateStatElementAsync("ENERGY", pendingStats.Energy.Value, ENERGYBONUS, 2, pendingStats.Energy.Level);
            await CreateStatElementAsync("MOVE SPEED", pendingStats.MovementSpeed.Value, SPEEDBONUS, 3, pendingStats.MovementSpeed.Level);
            await CreateStatElementAsync("DASH RANGE", pendingStats.DashRange.Value, DASHRANGEBONUS, 4, pendingStats.DashRange.Level);

            await CreateHeaderElementAsync("TURRET");
            await CreateStatElementAsync("FIRE RATE", pendingStats.FireRate.Value, -FIRERATEBONUS, 5, pendingStats.FireRate.Level);

            await CreateHeaderElementAsync("TURRET PROJECTILE");
            var stats = pendingProjectileStats.GetStatsForType(ProjectileType.Bullet);
            await CreateStatElements(ProjectileType.Bullet, "DAMAGE", stats.AverageDamage.Value, PPROJECTILEDAMAGEBONUS, 2, stats.AverageDamage.Level);
            await CreateStatElements(ProjectileType.Bullet, "SPEED", stats.Speed.Value, PPROJECTILESPEEDBONUS, 3, stats.Speed.Level);
            await CreateStatElements(ProjectileType.Bullet, "RANGE", stats.Range.Value, PPROJECTILERANGEBONUS, 4, stats.Range.Level);

            await Task.Yield();
            scrollbar.value = 1f;
        }

        private async Task CreateHeaderElementAsync(string headerText)
        {
            var headerElement = await uiFactory.CreateUpgradesHeaderItemAsync();
            if (headerElement == null) return;

            headerElement.transform.SetParent(parent, false);
            headerElement.transform.localScale = Vector3.one;
            var textMesh = headerElement.GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
                textMesh.text = headerText;
        }

        private async Task CreateStatElementAsync(string name, float baseValue, float bonus, int price, int currentLevel)
        {
            var element = await uiFactory.CreateUpgradesItemAsync();
            if (element == null) return;

            element.transform.SetParent(parent, false);
            element.transform.localScale = Vector3.one;
            element.Initialize(name, baseValue, bonus, price, currentLevel, () => OnStatUpgraded(), () => OnStatDowngraded());

            AssingClickSoundToWindow();

            statItems.Add(element);
        }

        private async Task CreateStatElements(ProjectileType type, string name, float baseValue, float bonus, int price, int currentLevel)
        {
            var element = await uiFactory.CreateUpgradesItemAsync();
            if (element == null) return;

            element.transform.SetParent(parent, false);
            element.transform.localScale = Vector3.one;

            var stats = pendingProjectileStats.GetStatsForType(type);
            element.Initialize(name, baseValue, bonus, price, currentLevel, () => OnProjectileStatUpgraded(type), () => OnProjectileStatDowngraded(type));

            AssingClickSoundToWindow();

            statItems.Add(element);
        }

        private void AssingClickSoundToWindow()
        {
            UIClickSound[] clickSounds = GetComponentsInChildren<UIClickSound>();
            if (clickSounds != null && clickSounds.Length > 0)
                foreach (var clickSound in clickSounds)
                    clickSound.SetWindow(window);
        }

        private void UpdateExistingElements()
        {
            foreach (var item in statItems)
            {
                item.UpdateStats(
                    GetCurrentBaseValue(item.StatName),
                    item.BonusPerUpgrade,
                    item.BasePrice
                );
                item.UpdateUI();
            }
        }

        private void UpdateStatLevelInProgress(string statName, int level)
        {
            var statsLevels = progressProvider.PlayerProgress.PrototypeStats;
            statsLevels.SetLevelForStat(statName, level);
        }

        private float GetCurrentBaseValue(string statName)
        {
            switch (statName)
            {
                case "HEALTH": return pendingStats.Health.Value;
                case "ENERGY": return pendingStats.Energy.Value;
                case "FIRE RATE": return pendingStats.FireRate.Value;
                case "MOVE SPEED": return pendingStats.MovementSpeed.Value;
                case "DASH RANGE": return pendingStats.DashRange.Value;

                default: return 0;
            }
        }

        private void UpdatePrices()
        {
            int totalPrice = 0;
            foreach (var item in statItems)
                totalPrice += item.TotalPrice;
            UpdateAvailableElements();
        }

        private async void OnStatUpgraded()
        {
            sfxPlayer?.PlaySFX(await assetProvider.Load<AudioClip>(AssetAddress.ClickButtonSound), 0.5f, 1, 1);
            UpdatePendingStats();
            UpdatePrices();
            pendingStats.IsChanged();
        }

        private async void OnStatDowngraded()
        {
            sfxPlayer?.PlaySFX(await assetProvider.Load<AudioClip>(AssetAddress.ClickButtonSound), 0.5f, 1, 1);
            UpdatePendingStats();
            UpdatePrices();
            pendingStats.IsChanged();
        }

        private void UpdatePendingStats()
        {
            foreach (var item in statItems)
            {
                switch (item.StatName)
                {
                    case "HEALTH":
                        pendingStats.Health.Value = item.CurrentValue;
                        pendingStats.Health.Level = item.CurrentLevel;
                        break;
                    case "ENERGY":
                        pendingStats.Energy.Value = item.CurrentValue;
                        pendingStats.Energy.Level = item.CurrentLevel;
                        break;
                    case "FIRE RATE":
                        pendingStats.FireRate.Value = item.CurrentValue;
                        pendingStats.FireRate.Level = item.CurrentLevel;
                        break;
                    case "MOVE SPEED":
                        pendingStats.MovementSpeed.Value = item.CurrentValue;
                        pendingStats.MovementSpeed.Level = item.CurrentLevel;
                        break;
                    case "DASH RANGE":
                        pendingStats.DashRange.Value = item.CurrentValue;
                        pendingStats.DashRange.Level = item.CurrentLevel;
                        break;
                }
            }
        }

        private void OnProjectileStatUpgraded(ProjectileType type)
        {
            UpdateProjectileStats(type);
            UpdatePrices();
            pendingProjectileStats.IsChanged();
        }

        private void OnProjectileStatDowngraded(ProjectileType type)
        {
            UpdateProjectileStats(type);
            UpdatePrices();
            pendingProjectileStats.IsChanged();
        }

        private void UpdateProjectileStats(ProjectileType type)
        {
            foreach (var item in statItems)
            {
                switch (item.StatName)
                {
                    case "DAMAGE":
                        pendingProjectileStats.TypeStats[type].AverageDamage.Value = item.CurrentValue;
                        pendingProjectileStats.TypeStats[type].AverageDamage.Level = item.CurrentLevel;
                        break;
                    case "SPEED":
                        pendingProjectileStats.TypeStats[type].Speed.Value = item.CurrentValue;
                        pendingProjectileStats.TypeStats[type].Speed.Level = item.CurrentLevel;
                        break;
                    case "RANGE":
                        pendingProjectileStats.TypeStats[type].Range.Value = item.CurrentValue;
                        pendingProjectileStats.TypeStats[type].Range.Level = item.CurrentLevel;
                        break;
                }
            }
        }

        private void OnCoinValueChanged(int value) => UpdateAvailableElements();

        private void OnDestroy()
        {
            if (progressProvider?.PlayerProgress?.PrototypeStats != null)
                progressProvider.PlayerProgress.PrototypeStats.Changed -= UpdateAvailableElements;
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
                progressProvider.PlayerProgress.PrototypeInventoryData.CoinValueChanged -= OnCoinValueChanged;
            ClearContainer();
        }

        public void RevertChanges()
        {
            int coinsToReturn = 0;
            pendingStats.CopyFrom(progressProvider.PlayerProgress.PrototypeStats);
            pendingProjectileStats.CopyFrom(progressProvider.PlayerProgress.ProjectileStats);

            foreach (var item in statItems)
            {
                if (IsPrototypeStat(item.StatName))
                {
                    coinsToReturn = CalculateCoinsToReturnForPrototypeStats(coinsToReturn, item);
                }
                else
                {
                    coinsToReturn = CalculateCoinsToReturnForProjectileStats(coinsToReturn, item);
                }
                item.ResetUpgrades();
            }

            if (coinsToReturn > 0)
                progressProvider.PlayerProgress.PrototypeInventoryData.CoinAmount += coinsToReturn;

            UpdateAvailableElements();
            pendingStats.IsChanged();
            pendingProjectileStats.IsChanged();
        }

        private int CalculateCoinsToReturnForProjectileStats(int coinsToReturn, UpgradesItem item)
        {
            ProjectileType type = ProjectileType.Bullet;

            int originalLevel = progressProvider.PlayerProgress.ProjectileStats.GetLevelForStat(type, item.StatName);
            float baseValue = ReturnStatsValue(ref coinsToReturn, item, originalLevel);

            item.Initialize(
                item.StatName,
                baseValue,
                item.BonusPerUpgrade,
                item.BasePrice,
                originalLevel,
                () => OnProjectileStatUpgraded(type),
                () => OnProjectileStatDowngraded(type)
            );
            return coinsToReturn;
        }

        private float ReturnStatsValue(ref int coinsToReturn, UpgradesItem item, int originalLevel)
        {
            if (item.PendingUpgrades > 0)
            {
                int firstUpgradeCost = originalLevel * item.BasePrice;
                int lastUpgradeCost = (originalLevel + item.PendingUpgrades - 1) * item.BasePrice;
                coinsToReturn += item.PendingUpgrades * (firstUpgradeCost + lastUpgradeCost) / 2;
            }

            float baseValue = GetCurrentBaseValue(item.StatName);
            return baseValue;
        }

        private int CalculateCoinsToReturnForPrototypeStats(int coinsToReturn, UpgradesItem item)
        {
            int originalLevel = progressProvider.PlayerProgress.PrototypeStats.GetLevelForStat(item.StatName);
            float baseValue = ReturnStatsValue(ref coinsToReturn, item, originalLevel);

            item.Initialize(
                item.StatName,
                baseValue,
                item.BonusPerUpgrade,
                item.BasePrice,
                originalLevel,
                () => OnStatUpgraded(),
                () => OnStatDowngraded()
            );
            return coinsToReturn;
        }

        private bool IsPrototypeStat(string statName)
        {
            switch (statName)
            {
                case "HEALTH": return true;
                case "ENERGY": return true;
                case "FIRE RATE": return true;
                case "MOVE SPEED": return true;
                case "DASH RANGE": return true;
                default: return false;
            };
        }
    }
}