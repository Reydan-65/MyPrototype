using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.UI.Elements;
using System;
using System.Collections.Generic;
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

        [SerializeField] private Transform parent;
        [SerializeField] private Scrollbar scrollbar;

        private List<UpgradeStatsItem> statItems = new List<UpgradeStatsItem>();
        private UpgradeStatsWindow window;
        private PrototypeStats pendingStats;
        private bool isInitialized;

        public List<UpgradeStatsItem> StatItems => statItems;
        public PrototypeStats PendingStats => pendingStats;

        private IUIFactory uiFactory;
        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IUIFactory uiFactory, IProgressProvider progressProvider)
        {
            this.uiFactory = uiFactory;
            this.progressProvider = progressProvider;
        }

        public void Initialize(UpgradeStatsWindow window)
        {
            if (isInitialized) return;

            this.window = window;

            isInitialized = true;

            pendingStats = new PrototypeStats();
            pendingStats.CopyFrom(progressProvider.PlayerProgress.PrototypeStats);

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

        private void CreateStatElements()
        {
            CreateStatElementAsync("HEALTH", pendingStats.Health.Value, HEALTHBONUS, 1, pendingStats.GetLevelForStat("HEALTH"));
            CreateStatElementAsync("ENERGY", pendingStats.Energy.Value, ENERGYBONUS, 2, pendingStats.GetLevelForStat("ENERGY"));
            CreateStatElementAsync("FIRE RATE", pendingStats.ShootingRate.Value, -FIRERATEBONUS, 5, pendingStats.GetLevelForStat("FIRE RATE"));
            CreateStatElementAsync("SPEED", pendingStats.MovementSpeed.Value, SPEEDBONUS, 3, pendingStats.GetLevelForStat("SPEED"));
            CreateStatElementAsync("DASH RANGE", pendingStats.DashRange.Value, DASHRANGEBONUS, 4, pendingStats.GetLevelForStat("DASH RANGE"));
        }

        private async void CreateStatElementAsync(string name, float baseValue, float bonus, int price, int currentLevel)
        {
            var element = await uiFactory.CreateUpgradeStatsItemAsync();
            if (element == null) return;

            element.transform.SetParent(parent, false);
            element.transform.localScale = Vector3.one;
            element.Initialize(name, baseValue, bonus, price, currentLevel, () => OnStatUpgraded(), () => OnStatDowngraded());

            statItems.Add(element);
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
                case "FIRE RATE": return pendingStats.ShootingRate.Value;
                case "SPEED": return pendingStats.MovementSpeed.Value;
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

        private void OnStatUpgraded()
        {
            UpdatePendingStats();
            UpdatePrices();
            pendingStats.IsChanged();
        }

        private void OnStatDowngraded()
        {
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
                        pendingStats.ShootingRate.Value = item.CurrentValue;
                        pendingStats.ShootingRate.Level = item.CurrentLevel;
                        break;
                    case "SPEED":
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

            foreach (var item in statItems)
            {
                int originalLevel = progressProvider.PlayerProgress.PrototypeStats.GetLevelForStat(item.StatName);

                if (item.PendingUpgrades > 0)
                {
                    int firstUpgradeCost = originalLevel * item.BasePrice;
                    int lastUpgradeCost = (originalLevel + item.PendingUpgrades - 1) * item.BasePrice;
                    coinsToReturn += item.PendingUpgrades * (firstUpgradeCost + lastUpgradeCost) / 2;
                }

                float baseValue = GetCurrentBaseValue(item.StatName);

                item.Initialize(
                    item.StatName,
                    baseValue,
                    item.BonusPerUpgrade,
                    item.BasePrice,
                    originalLevel,
                    () => OnStatUpgraded(),
                    () => OnStatDowngraded()
                );

                item.ResetUpgrades();
            }

            if (coinsToReturn > 0)
                progressProvider.PlayerProgress.PrototypeInventoryData.CoinAmount += coinsToReturn;

            UpdateAvailableElements();
            pendingStats.IsChanged();
        }
    }
}