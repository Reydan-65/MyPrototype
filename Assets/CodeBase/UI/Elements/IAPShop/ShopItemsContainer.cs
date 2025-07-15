using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class ShopItemsContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] unavailableObjects;
        [SerializeField] private Transform parent;

        private IIApService service;
        private IProgressProvider progressProvider;
        private IUIFactory factory;

        [Inject]
        public void Construct(
            IIApService service,
            IProgressProvider progressProvider,
            IUIFactory factory)
        {
            this.service = service;
            this.progressProvider = progressProvider;
            this.factory = factory;
        }

        private void Start()
        {
            progressProvider.PlayerProgress.PurchaseData.Changed += UpdateAvailableItems;
            UpdateAvailableItems();
        }

        private void OnDestroy()
        {
            progressProvider.PlayerProgress.PurchaseData.Changed -= UpdateAvailableItems;
        }

        private void UpdateAvailableItems()
        {
            for (int i = 0; i < unavailableObjects.Length; i++)
            {
                unavailableObjects[i].SetActive(!service.IsInitialized);
            }

            if (service.IsInitialized == false) return;

            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }

            ProductDescription[] descriptions = service.GetProductDescription().ToArray();

            CreateShopItemsAsync(descriptions);
        }

        public async void CreateShopItemsAsync(ProductDescription[] descriptions)
        {
            if (parent == null) return;

            foreach (var description in descriptions)
            {
                ShopItem item = await factory.CreateShopItemAsync();
                if (item == null) continue;

                item.transform.SetParent(parent, false);
                item.gameObject.SetActive(true);
                item.Initialize(description);
            }
        }

    }
}
