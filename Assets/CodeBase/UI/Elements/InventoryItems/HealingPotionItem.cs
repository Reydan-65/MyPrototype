namespace CodeBase.GamePlay.UI
{
    public class HealingPotionItem : InventoryItem
    {
        private void OnValueChanged(int coinsValue) => UpdateAmountText();

        protected override void UpdateAmountText()
        {
            if (amountText != null && inventoryData != null)
                amountText.text = inventoryData.HealingPotionAmount.ToString();
        }

        protected override void SubscribeToValueChanges()
        {
            if (inventoryData != null)
                inventoryData.HealingPotionValueChanged += OnValueChanged;
        }

        protected override void UnsubscribeFromInventory()
        {
            if (inventoryData != null)
                inventoryData.HealingPotionValueChanged -= OnValueChanged;
        }
    }
}