using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsContainer : MonoBehaviour
    {
        private const int BonusMaxHitPoints = 5;
        private const int BonusDamage = 1;
        private const float BonusShootingRate = -0.05f;
        private const float BonusMovementSpeed = 0.1f;
        private const float BonusDashRange = 0.1f;

        [SerializeField] private Transform parent;

        private IUIFactory factory;
        private IProgressProvider progressProvider;
        private IProgressSaver progressSaver;

        [Inject]
        public void Construct(IUIFactory factory, IProgressProvider progressProvider, IProgressSaver progressSaver)
        {
            this.factory = factory;
            this.progressProvider = progressProvider;
            this.progressSaver = progressSaver;
        }

        private void Start()
        {
            progressProvider.PlayerProgress.HeroStats.Changed += UpdateAvailableElements;
            UpdateAvailableElements();
        }

        private void OnDestroy()
        {
            progressProvider.PlayerProgress.HeroStats.Changed -= UpdateAvailableElements;
        }

        private void UpdateAvailableElements()
        {
            ClearContainer();

            HeroStats stats = HeroStats.GetStats();
            CreateUpgradeStatsElementAsync(stats);
        }

        private void ClearContainer()
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);
        }

        public async void CreateUpgradeStatsElementAsync(HeroStats stats)
        {
            if (parent == null) return;

            await CreateStatElementAsync("HEALTH", stats.MaxHitPoints.ToString(), BonusMaxHitPoints.ToString(), 1.ToString(),
                () => UpgradeStat(ref stats.MaxHitPoints, BonusMaxHitPoints));
            await CreateStatElementAsync("DAMAGE", stats.Damage.ToString(), BonusDamage.ToString(), 1.ToString(),
                () => UpgradeStat(ref stats.Damage, BonusDamage));
            await CreateStatElementAsync("FIRE RATE", stats.ShootingRate.ToString(), BonusShootingRate.ToString(), 1.ToString(),
                () => UpgradeStat(ref stats.ShootingRate, BonusShootingRate));
            await CreateStatElementAsync("SPEED", stats.MovementSpeed.ToString(), BonusMovementSpeed.ToString(), 1.ToString(),
                () => UpgradeStat(ref stats.MovementSpeed, BonusMovementSpeed));
            await CreateStatElementAsync("DASH RANGE", stats.DashRange.ToString(), BonusDashRange.ToString(), 1.ToString(),
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
            });
        }

        private void UpgradeStat(ref int stat, int bonusValue)
        {
            stat += bonusValue;
            progressProvider.PlayerProgress.HeroStats.IsChanged();
        }

        private void UpgradeStat(ref float stat, float bonusValue)
        {
            stat += bonusValue;
            progressProvider.PlayerProgress.HeroStats.IsChanged();
        }
    }
}