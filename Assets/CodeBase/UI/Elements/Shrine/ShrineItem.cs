using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class ShrineItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button itemButton;

        private IWindowsProvider windowsProvider;

        [Inject]
        public void Construct(IWindowsProvider windowsProvider)
        {
            this.windowsProvider = windowsProvider;
        }

        private void OnDestroy()
        {
            itemButton.onClick.RemoveListener(OnItemButtonClicked);
        }

        public void Initialize()
        {
            titleText.text = "UPGRADE STATS";

            itemButton.onClick.RemoveListener(OnItemButtonClicked);
            itemButton.onClick.AddListener(OnItemButtonClicked);
        }

        private void OnItemButtonClicked()
        {
            windowsProvider.Open(WindowID.UpgradesWindow);
        }
    }
}