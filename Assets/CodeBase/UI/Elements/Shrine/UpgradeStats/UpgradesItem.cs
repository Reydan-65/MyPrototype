using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class UpgradesItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI currentValueText;
        [SerializeField] private TextMeshProUGUI bonusValueText;
        [SerializeField] private TextMeshProUGUI costValueText;
        [SerializeField] private Button plusButton;
        [SerializeField] private Button minusButton;
        [SerializeField] private Color[] itemButtonColors;

        private float baseValue;
        private float currentValue;
        private int currentLevel;
        private bool isProcessingClick;
        private float bonusPerUpgrade;
        private int basePrice;

        private System.Action onUpgrade;
        private System.Action onDowngrade;

        private string statName;
        public int PendingUpgrades { get; private set; }
        public int CurrentPrice { get; private set; }
        public float TotalBonusValue => PendingUpgrades * bonusPerUpgrade;
        public int TotalPrice => currentLevel * basePrice;

        public string StatName => statName;
        public float BaseValue => baseValue;
        public float CurrentValue => currentValue;
        public int CurrentLevel => currentLevel;
        public float BonusPerUpgrade => bonusPerUpgrade;
        public int BasePrice { get => basePrice; set => basePrice = value; }

        private IProgressProvider progress;

        [Inject]
        public void Construct(IProgressProvider progress) => this.progress = progress;

        public void Initialize(string statName,
            float baseValue,
            float bonusPerUpgrade,
            int basePrice,
            int currentLevel,
            System.Action onUpgrade,
            System.Action onDowngrade)
        {
            this.statName = statName;
            this.baseValue = baseValue;
            this.bonusPerUpgrade = bonusPerUpgrade;
            this.basePrice = basePrice;
            this.currentLevel = currentLevel;
            this.onUpgrade = onUpgrade;
            this.onDowngrade = onDowngrade;

            currentValue = baseValue;

            UpdateUI();
        }

        private void OnEnable()
        {
            plusButton.onClick.AddListener(OnPlusClicked);
            minusButton.onClick.AddListener(OnMinusClicked);
        }

        private void OnDisable()
        {
            plusButton.onClick.RemoveListener(OnPlusClicked);
            minusButton.onClick.RemoveListener(OnMinusClicked);
        }

        private void OnPlusClicked()
        {
            if (isProcessingClick) return;
            isProcessingClick = true;

            PendingUpgrades++;
            currentLevel++;
            currentValue += bonusPerUpgrade;
            progress.PlayerProgress.PrototypeInventoryData.CoinAmount -= basePrice * (currentLevel - 1);
            onUpgrade?.Invoke();
            UpdateUI();

            isProcessingClick = false;
        }

        private void OnMinusClicked()
        {
            if (PendingUpgrades > 0)
            {
                PendingUpgrades--;
                currentLevel--;
                currentValue -= bonusPerUpgrade;
                progress.PlayerProgress.PrototypeInventoryData.CoinAmount += basePrice * currentLevel;
                onDowngrade?.Invoke();
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            statNameText.text = StatName;
            currentValueText.text = currentValue.ToString("F2");
            bonusValueText.text = TotalBonusValue > 0 ? $"+{TotalBonusValue:F2}" : "0";
            costValueText.text = TotalPrice.ToString();

            minusButton.interactable = PendingUpgrades > 0;
            plusButton.interactable = CanAffordNextUpgrade();

            UpdateButtonsColor();
        }

        public void UpdateStats(float newBaseValue, float newBonus, int newBasePrice)
        {
            this.baseValue = newBaseValue;
            this.bonusPerUpgrade = newBonus;
            this.basePrice = newBasePrice;
        }

        private void UpdateButtonsColor()
        {
            Image plusImage = plusButton.GetComponent<Image>();
            if (plusImage != null)
                plusImage.color = plusButton.interactable ? itemButtonColors[0] : itemButtonColors[1];
            Image minusImage = minusButton.GetComponent<Image>();
            if (minusImage != null)
                minusImage.color = minusButton.interactable ? itemButtonColors[0] : itemButtonColors[1];
        }

        public void ApplyUpgrades()
        {
            baseValue += TotalBonusValue;
            PendingUpgrades = 0;
            UpdateUI();
        }

        public void ResetUpgrades()
        {
            PendingUpgrades = 0;
            UpdateUI();
        }

        private bool CanAffordNextUpgrade()
            => progress.PlayerProgress.PrototypeInventoryData.CoinAmount >= basePrice * currentLevel;
    }
}