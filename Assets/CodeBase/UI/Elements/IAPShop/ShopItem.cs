using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Button buyItemButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI quantityText;

        private IIApService service;
        private string productID;

        [Inject]
        public void Construct(IIApService service)
        {
            this.service = service;
        }

        private void OnDestroy()
        {
            buyItemButton.onClick.RemoveListener(OnBuyButtonClicked);

        }
        public void Initialize(ProductDescription description)
        {
            if (description == null) return;

            productID = description.ID;
            titleText.text = description.ProductConfig.Title;
            priceText.text = "$" + description.ProductConfig.Price.ToString();
            quantityText.text = description.ProductConfig.Quantity.ToString();

            if (description.ProductConfig.Quantity == 0)
                quantityText.enabled = false;

            buyItemButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            service.StartPurchase(productID);
        }
    }
}
