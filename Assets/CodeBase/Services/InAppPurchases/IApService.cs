using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services
{
    public class IApService : IIApService
    {
        private const string NoAdsID = "no_ads";
        private const string SmallCoinsPackID = "small_coins_pack";
        private const string FemaleSkinID = "female_skin";

        private IProgressProvider progressProvider;
        private IProgressSaver progressSaver;

        private UnityIApPrivider unityIApPrivider;

        public bool IsInitialized => unityIApPrivider.IsInitialized;

        public event UnityAction Initialized;

        public IApService(IProgressProvider progressProvider, IProgressSaver progressSaver)
        {
            this.progressProvider = progressProvider;
            this.progressSaver = progressSaver;
        }

        public void Initialize()
        {
            unityIApPrivider = new UnityIApPrivider();
            unityIApPrivider.Initialize(this);
            unityIApPrivider.Initialized += () => Initialized?.Invoke();
        }

        public void StartPurchase(string productID)
        {
            unityIApPrivider.StartPurchase(productID);
        }

        public void ProcessPurchase(Product product)
        {
            ProductConfig config = unityIApPrivider.ProductConfigs.Find(x => x.ID == product.definition.id);

            progressProvider.PlayerProgress.PurchaseData.AddPurchase(config.ID);

            PurchaseProcessing(config);

            progressSaver.SaveProgress();
        }

        public List<ProductDescription> GetProductDescription()
        {
            List<ProductDescription> descriptions = new List<ProductDescription>();
            for (int i = 0; i < unityIApPrivider.ProductConfigs.Count; i++)
            {
                ProductConfig config = unityIApPrivider.ProductConfigs[i];
                BoughtIAP boughtIAP = progressProvider.PlayerProgress.PurchaseData.BoughtIAPs.Find(x => x.ProductID == config.ID);

                if (boughtIAP != null && config.Type == ProductType.NonConsumable) continue;

                ProductDescription description = new ProductDescription()
                {
                    ID = config.ID,
                    ProductConfig = config,
                    Product = unityIApPrivider.Products[i]
                };

                descriptions.Add(description);
            }

            return descriptions;
        }

        private void PurchaseProcessing(ProductConfig config)
        {
            if (config.ID == SmallCoinsPackID)
            {
                progressProvider.PlayerProgress.HeroInventoryData.AddCoin(config.Quantity);
            }

            if (config.ID == FemaleSkinID)
            {
                progressProvider.PlayerProgress.PurchaseData.IsFemaleSkinUnlocked = true;
            }

            if (config.ID == NoAdsID)
            {
                // TODO
            }
        }
    }
}
