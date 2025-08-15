namespace CodeBase.GamePlay.UI
{
    public class CoinItem : InventoryItem
    {
        private void OnValueChanged(int coinsValue) => UpdateAmountText();

        protected override void UpdateAmountText()
        {
            if (amountText != null && inventoryData != null)
                amountText.text = inventoryData.CoinAmount.ToString();
        }

        protected override void SubscribeToValueChanges()
        {
            if (inventoryData != null)
                inventoryData.CoinValueChanged += OnValueChanged;
        }

        protected override void UnsubscribeFromInventory()
        {
            if (inventoryData != null)
                inventoryData.CoinValueChanged -= OnValueChanged;
        }
    }
}