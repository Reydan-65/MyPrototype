using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeInventory : MonoBehaviour
    {
        private IProgressProvider progressProvider;

        private PrototypeInventoryData inventoryData;
        public event UnityAction<int> OnCoinAmountChanged;
        public event UnityAction<string> KeyAdded;
        public event UnityAction<string> KeyRemoved;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;
            Initialize();
        }

        private void Initialize()
        {
            if (progressProvider?.PlayerProgress?.PrototypeInventoryData == null) return;

            inventoryData = progressProvider.PlayerProgress.PrototypeInventoryData;
            inventoryData.CoinValueChanged += OnCoinChanged;
            inventoryData.KeyAdded += OnKeyAddedHandler;
            inventoryData.KeyRemoved += OnKeyRemovedHandler;
            OnCoinChanged(inventoryData.CoinAmount);
        }

        private void OnCoinChanged(int newValue)
        {
            OnCoinAmountChanged?.Invoke(newValue);
        }

        private void OnKeyAddedHandler(string keyId)
        {
            KeyAdded?.Invoke(keyId);
        }

        private void OnKeyRemovedHandler(string keyId)
        {
            KeyRemoved?.Invoke(keyId);
        }

        private void OnDestroy()
        {
            if (inventoryData != null)
            {
                inventoryData.CoinValueChanged -= OnCoinChanged;
                inventoryData.KeyAdded -= OnKeyAddedHandler;
                inventoryData.KeyRemoved -= OnKeyRemovedHandler;
            }
        }

        public void SyncWithData(PrototypeInventoryData newInventoryData)
        {
            if (newInventoryData == null || inventoryData== null) return;

            int oldCoins = inventoryData.CoinAmount;
            
            if (oldCoins != inventoryData.CoinAmount)
            {
                OnCoinChanged(inventoryData.CoinAmount);
            }

            foreach (var keyId in newInventoryData.Keys)
            {
                if (!inventoryData.HasKey(keyId))
                {
                    OnKeyAddedHandler(keyId);
                }
            }

            foreach (var keyId in inventoryData.Keys)
            {
                if (!newInventoryData.HasKey(keyId))
                {
                    OnKeyRemovedHandler(keyId);
                }
            }
        }

        public bool HasKey(string keyId) => inventoryData?.HasKey(keyId) ?? false;
    }
}