using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.UI
{
    public class ShrineWindow : WindowBase
    {
        public event UnityAction UpgradeStatsButtonClicked;

        [SerializeField] private Button updateStatsButton;

        private void Start()
        {
            updateStatsButton.onClick.AddListener(() => UpgradeStatsButtonClicked?.Invoke());
        }

        protected override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
