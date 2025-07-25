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
            if (progressProvider?.PlayerProgress?.HeroStats != null)
                progressProvider.PlayerProgress.HeroStats.Changed -= UpdateAvailableElements;
        }

        public void Initialize(UpgradeStatsWindow window)
        {
            if (isInitialized) return;

            this.window = window;
            isInitialized = true;

            progressProvider.PlayerProgress.HeroStats.Changed -= UpdateAvailableElements;
            progressProvider.PlayerProgress.HeroStats.Changed += UpdateAvailableElements;

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

        private async void CreateUpgradeStatsElementAsync(HeroStats stats)
        {
            if (parent == null) return;

            await CreateStatElementAsync("HEALTH", stats.MaxHitPoints.ToString(), BonusMaxHitPoints.ToString(), "1",
                () => UpgradeStat(ref stats.MaxHitPoints, BonusMaxHitPoints));
            await CreateStatElementAsync("ENERGY", stats.MaxEnergy.ToString(), BonusMaxEnergy.ToString(), "1",
                () => UpgradeStat(ref stats.MaxEnergy, BonusMaxEnergy));
            await CreateStatElementAsync("FIRE RATE", stats.ShootingRate.ToString("F2"), Mathf.Abs(BonusShootingRate).ToString("F2"), "1",
                () => UpgradeStat(ref stats.ShootingRate, BonusShootingRate));
            await CreateStatElementAsync("SPEED", stats.MovementSpeed.ToString("F2"), BonusMovementSpeed.ToString("F2"), "1",
                () => UpgradeStat(ref stats.MovementSpeed, BonusMovementSpeed));
            await CreateStatElementAsync("DASH RANGE", (stats.DashRange / 100).ToString("F2"), (BonusDashRange / 100).ToString("F2"), "1",
                () => UpgradeStat(ref stats.DashRange, BonusDashRange));
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
            element.Initialize(statName, currentValue, bonusValue, priceValue, () =>
            {
                upgradeAction.Invoke();
                window.UpdateStatsDisplay();
            });
        }

        private void UpgradeStat(ref int stat, int bonusValue)
        {
            stat += bonusValue;
            window.UpdateStatsDisplay();
        }

        private void UpgradeStat(ref float stat, float bonusValue)
        {
            stat += bonusValue;
            window.UpdateStatsDisplay();
        }
    }
}