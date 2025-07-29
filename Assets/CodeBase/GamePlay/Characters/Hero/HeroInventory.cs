using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Hero
{
    public class HeroInventory : MonoBehaviour
    {
        private IProgressProvider progressProvider;

        private HeroInventoryData inventoryData;
        public event UnityAction<int> OnCoinAmountChanged;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;
            Initialize();
        }

        private void Initialize()
        {
            if (progressProvider?.PlayerProgress?.HeroInventoryData == null) return;

            inventoryData = progressProvider.PlayerProgress.HeroInventoryData;
            inventoryData.CoinValueChanged += OnCoinChanged;
            OnCoinChanged(inventoryData.CoinAmount);
        }

        private void OnCoinChanged(int newValue)
        {
            OnCoinAmountChanged?.Invoke(newValue);
        }

        private void OnDestroy()
        {
            if (inventoryData != null)
                inventoryData.CoinValueChanged -= OnCoinChanged;
        }

        public void SyncWithData(HeroInventoryData newInventoryData)
        {
            if (newInventoryData == null || inventoryData== null) return;

            int oldCoins = inventoryData.CoinAmount;
            bool oldKeyState = inventoryData.HasKey;

            if (oldCoins != inventoryData.CoinAmount)
            {
                OnCoinChanged(inventoryData.CoinAmount);
            }
        }
    }
}