using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using TMPro;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;

        private IProgressProvider progressProvider;
        private HeroInventoryData inventoryData;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;

            if (this.progressProvider?.PlayerProgress?.HeroInventoryData != null)
                SubscribeToInventory();
        }

        private void Start()
        {
            if (inventoryData == null && progressProvider?.PlayerProgress?.HeroInventoryData != null)
                SubscribeToInventory();

            UpdateCoinAmountText();
        }

        private void SubscribeToInventory()
        {
            if (inventoryData != null)
                inventoryData.CoinValueChanged -= OnCoinChanged;

            inventoryData = progressProvider.PlayerProgress.HeroInventoryData;
            inventoryData.CoinValueChanged += OnCoinChanged;
        }

        private void OnDestroy()
        {
            if (inventoryData != null)
                inventoryData.CoinValueChanged -= OnCoinChanged;
        }

        private void OnCoinChanged(int coinsValue)
        {
            UpdateCoinAmountText();
        }

        private void UpdateCoinAmountText()
        {
            if (amountText != null && inventoryData != null)
                amountText.text = inventoryData.CoinAmount.ToString();
        }
    }
}