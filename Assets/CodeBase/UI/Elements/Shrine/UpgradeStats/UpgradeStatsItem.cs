using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI currentValueText;
        [SerializeField] private TextMeshProUGUI bonusValueText;
        [SerializeField] private TextMeshProUGUI priceValueText;
        [SerializeField] private Button upgradeButton;

        public string StatName { get; private set; }

        public void Initialize(string statName,
            string currentValue,
            string bonusValue,
            int priceValue,
            System.Action onUpgrade)
        {
            StatName = statName;
            statNameText.text = statName;
            currentValueText.text = currentValue;
            bonusValueText.text = "+"+bonusValue;
            priceValueText.text = priceValue.ToString();

            upgradeButton.onClick.RemoveListener(() => onUpgrade?.Invoke());
            upgradeButton.onClick.AddListener(() => onUpgrade?.Invoke());
        }

        public void UpdateCurrentValue(string newValue)
        {
            currentValueText.text = newValue;
        }

        public void UpdatePriceValue(string newValue)
        {
            priceValueText.text = newValue;
        }

        public void SetInteractable(bool interactable)
        {
            upgradeButton.interactable = interactable;
        }

        public void SetPriceColor(Color color)
        {
            priceValueText.color = color;
        }
    }
}