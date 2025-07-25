using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using TMPro;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public abstract class InventoryItem : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI amountText;

        private IProgressProvider progressProvider;
        protected HeroInventoryData inventoryData;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;

            if (this.progressProvider?.PlayerProgress?.HeroInventoryData != null)
            {
                inventoryData = progressProvider.PlayerProgress.HeroInventoryData;
                SubscribeToValueChanges();
            }
        }

        private void Start()
        {
            if (inventoryData == null && 
                progressProvider?.PlayerProgress?.HeroInventoryData != null)
                SubscribeToValueChanges();

            UpdateAmountText();
        }

        protected virtual void SubscribeToValueChanges() { }
        protected virtual void UnsubscribeFromInventory() { }
        protected virtual void UpdateAmountText() { }

        private void OnDestroy()
        {
            UnsubscribeFromInventory();
        }
    }
}