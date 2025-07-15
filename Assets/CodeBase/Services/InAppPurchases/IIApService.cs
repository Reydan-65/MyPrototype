using CodeBase.Infrastructure.DependencyInjection;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services
{
    public interface IIApService : IService
    {
        public void Initialize();
        bool IsInitialized { get; }
        event UnityAction Initialized;
        public void StartPurchase(string productID);
        void ProcessPurchase(Product purchasedProduct);
        List<ProductDescription> GetProductDescription();
    }
}
