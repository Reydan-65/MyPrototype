using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class UpgradeStatsWindow : WindowBase
    {
        public event UnityAction AcceptButtonClicked;
        public event UnityAction CancelButtonClicked;

        [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TextMeshProUGUI cancelButtonText;

        private UpgradeStatsContainer statsContainer;
        private UpgradeStatsPresenter statsPresenter;

        private IInputService inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            this.inputService = inputService;
        }

        private void Start()
        {
            inputService.Enable = false;

            acceptButton.onClick.AddListener(() => AcceptButtonClicked?.Invoke());
            cancelButton.onClick.AddListener(() => CancelButtonClicked?.Invoke());
        }

        protected override void OnClose()
        {
            inputService.Enable = true;
            Destroy(gameObject);
        }

        public HeroStats GetNewStats()
        {
            if(statsPresenter != null) return statsPresenter.NewStats;
            return null;
        }

        public void InitializeContainer(UpgradeStatsPresenter presenter)
        {
            statsPresenter = presenter;
            statsContainer = GetComponent<UpgradeStatsContainer>();

            if (statsContainer != null)
                statsContainer.Initialize(this);
        }

        public void UpdateStatsDisplay()
        {
            if (statsContainer != null)
                statsContainer.UpdateAvailableElements();
        }

        public void SetCancelButtonText(string text)
        {
            cancelButtonText.text = text;
        }
    }
}
