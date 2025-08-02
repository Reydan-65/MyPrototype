using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class ShrineWindow : WindowBase
    {
        public event UnityAction UpgradeStatsButtonClicked;
        public event UnityAction ShrineWindowClosed;

        [SerializeField] private Button updateStatsButton;

        private IInputService inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            this.inputService = inputService;
        }

        private void Start()
        {
            inputService.Enable = false;

            updateStatsButton.onClick.AddListener(() => UpgradeStatsButtonClicked?.Invoke());
        }

        protected override void OnClose()
        {
            inputService.Enable = true;
            ShrineWindowClosed?.Invoke();
            Destroy(gameObject);
        }
    }
}
