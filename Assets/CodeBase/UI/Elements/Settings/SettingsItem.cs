using CodeBase.Data;
using CodeBase.GamePlay.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class SettingsItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Slider slider;

        private UIClickSound sliderClickSound;
        public SettingItemID ID { get; private set; }

        private void Awake() =>sliderClickSound = GetComponent<UIClickSound>();

        public void Initialize(SettingItemID id, string title)
        {
            ID = id;
            titleText.text = title;
        }

        public void UpdateCurrentValue(string newValue) =>valueText.text = newValue;
        public Slider GetSettingSlider() => slider ?? null;

        public void SetWindow(WindowBase window)
        {
            if (sliderClickSound != null)
                sliderClickSound.SetWindow(window);
        }
    }
}
