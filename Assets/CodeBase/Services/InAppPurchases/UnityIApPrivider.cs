using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace CodeBase.Infrastructure.Services
{
    public class UnityIApPrivider : IDetailedStoreListener
    {
        public event UnityAction Initialized;
        public bool IsInitialized => controller != null && extensions != null;

        public List<ProductConfig> ProductConfigs = new List<ProductConfig>();
        public List<Product> Products = new List<Product>();

        private IStoreController controller;
        private IExtensionProvider extensions;
        private IIApService service;

        public void Initialize(IIApService service)
        {
            this.service = service;

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                builder.AddProduct(item.id, item.type);

                ProductConfigs.Add(new ProductConfig()
                {
                    ID = item.id,
                    Type = item.type,
                    Quantity = GetQuantity(item),
                    Price = item.googlePrice.value.ToString(),
                    Title = item.defaultDescription.Title
                });
            }

            UnityPurchasing.Initialize(this, builder);
        }

        private int GetQuantity(ProductCatalogItem item)
        {
            int quantity = 0;

            foreach (ProductCatalogPayout payout in item.Payouts)
            {
                quantity = (int)payout.quantity;
            }

            return quantity;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            this.controller = controller;
            this.extensions = extensions;

            for (int i = 0; i < controller.products.all.Length; i++)
            {
                Products.Add(controller.products.all[i]);
            }

            Initialized?.Invoke();

            //Debug.Log("IAp Initialized!");
        }

        public void StartPurchase(string productID)
        {
            controller.InitiatePurchase(productID);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            service.ProcessPurchase(purchaseEvent.purchasedProduct);

            return PurchaseProcessingResult.Complete;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log("OnInitializeFailed");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log("OnPurchaseFailed");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log("OnPurchaseFailed");
        }

    }
}
