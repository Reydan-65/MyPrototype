using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsContainer : MonoBehaviour
    {
        private const int BonusMaxHitPoints = 2;
        private const int BonusMaxEnergy = 1;
        private const float BonusShootingRate = -0.01f;
        private const float BonusMovementSpeed = 0.02f;
        private const float BonusDashRange = 2f;

        private const int BonusMaxHitPrice = 1;
        private const int BonusMaxEnergyPrice = 2;
        private const int BonusMovementSpeedPrice = 3;
        private const int BonusDashRangePrice = 4;
        private const int BonusShootingRatePrice = 5;

        [SerializeField] private Transform parent;

        private UpgradeStatsWindow window;
        private bool isInitialized;

        private IUIFactory factory;
        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IUIFactory factory, IProgressProvider progressProvider)
        {
            this.factory = factory;
            this.progressProvider = progressProvider;
        }

        private void OnDestroy()
        {
            if (progressProvider?.PlayerProgress?.PrototypeStats != null)
                progressProvider.PlayerProgress.PrototypeStats.Changed -= UpdateAvailableElements;

            if (progressProvider?.PlayerProgress?.PrototypeInventoryData != null)
                progressProvider.PlayerProgress.PrototypeInventoryData.CoinValueChanged -= OnCoinValueChanged;
        }

        public void Initialize(UpgradeStatsWindow window)
        {
            if (isInitialized) return;

            this.window = window;
            isInitialized = true;
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
        }

        public void UpdateAvailableElements()
        {
            ClearContainer();
            CreateUpgradeStatsElementAsync(window.GetNewStats());
        }

        private void ClearContainer()
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);
        }

        private async void CreateUpgradeStatsElementAsync(PrototypeStats stats)
        {
            if (parent == null) return;

            await CreateStatElementAsync("HEALTH", stats.MaxHitPoints.ToString(), BonusMaxHitPoints.ToString(), BonusMaxHitPrice.ToString(),
                () => UpgradeStat(ref stats.MaxHitPoints, BonusMaxHitPoints, BonusMaxHitPrice));
            await CreateStatElementAsync("ENERGY", stats.MaxEnergy.ToString(), BonusMaxEnergy.ToString(), BonusMaxEnergyPrice.ToString(),
                () => UpgradeStat(ref stats.MaxEnergy, BonusMaxEnergy, BonusMaxEnergyPrice));
            await CreateStatElementAsync("FIRE RATE", stats.ShootingRate.ToString("F2"), Mathf.Abs(BonusShootingRate).ToString("F2"), BonusShootingRatePrice.ToString(),
                () => UpgradeStat(ref stats.ShootingRate, BonusShootingRate, BonusShootingRatePrice));
            await CreateStatElementAsync("SPEED", stats.MovementSpeed.ToString("F2"), BonusMovementSpeed.ToString("F2"), BonusMovementSpeedPrice.ToString(),
                () => UpgradeStat(ref stats.MovementSpeed, BonusMovementSpeed, BonusMovementSpeedPrice));
            await CreateStatElementAsync("DASH RANGE", (stats.DashRange / 100).ToString("F2"), (BonusDashRange / 100).ToString("F2"), BonusDashRangePrice.ToString(),
                () => UpgradeStat(ref stats.DashRange, BonusDashRange, BonusDashRangePrice));
        }

        private async Task CreateStatElementAsync(
            string statName,
            string currentValue,
            string bonusValue,
            string priceValue,
            System.Action upgradeAction)
        {
            var element = await factory.CreateUpgradeStatsItemAsync();
            if (element == null) return;

            element.transform.SetParent(parent, false);
            element.gameObject.SetActive(true);
            element.Initialize(statName, currentValue, bonusValue, int.Parse(priceValue), () =>
            {
                upgradeAction.Invoke();
                window.UpdateStatsDisplay();
            });

            if (int.Parse(priceValue) > progressProvider.PlayerProgress.PrototypeInventoryData.CoinAmount)
                element.SetInteractable(false);
        }

        private void UpgradeStat(ref int stat, int bonusValue, int price)
        {
            stat += bonusValue;
            progressProvider.PlayerProgress.PrototypeInventoryData.CoinAmount -= price;
            window.UpdateStatsDisplay();
        }

        private void UpgradeStat(ref float stat, float bonusValue, int price)
        {
            stat += bonusValue;
            progressProvider.PlayerProgress.PrototypeInventoryData.CoinAmount -= price;
            window.UpdateStatsDisplay();
        }

        private void OnCoinValueChanged(int value)
        {
            UpdateAvailableElements();
        }
    }
}