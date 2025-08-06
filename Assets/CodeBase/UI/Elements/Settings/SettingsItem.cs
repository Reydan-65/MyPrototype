using CodeBase.Data;
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

        public SettingItemID ID { get; private set; }

        public void Initialize(SettingItemID id, string title)
        {
            ID = id;
            titleText.text = title;
        }

        public void UpdateCurrentValue(string newValue)
        {
            valueText.text = newValue;
        }

        public Slider GetSettingSlider() => slider ?? null;
    }
}
