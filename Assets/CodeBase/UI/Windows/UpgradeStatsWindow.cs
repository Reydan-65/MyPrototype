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
        public UnityAction AcceptButtonClicked;
        public UnityAction AdvancedCloseButtonClicked;
        public UnityAction YesButtonClicked;
        public UnityAction NoButtonClicked;

        [Header("Panels")]
        [SerializeField] private GameObject mainBottomPanel;
        [SerializeField] private GameObject confirmBottomPanel;

        [Header("Main Panel Buttons")]
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button advancedCloseButton;

        [Header("Confirm Panel Buttons")]
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private UpgradeStatsContainer statsContainer;
        private UpgradeStatsPresenter statsPresenter;
        public GameObject MainBottomPanel => mainBottomPanel;
        public GameObject ConfirmBottomPanel => confirmBottomPanel;
        public Button AcceptButton => acceptButton;
        private IInputService inputService;
        public UpgradeStatsContainer Container => statsContainer;

        [Inject]
        public void Construct(IInputService inputService)
        {
            this.inputService = inputService;
        }

        private void Start()
        {
            inputService.Enable = false;

            acceptButton.onClick.AddListener(() => AcceptButtonClicked?.Invoke());
            advancedCloseButton.onClick.AddListener(() => AdvancedCloseButtonClicked?.Invoke());
            yesButton.onClick.AddListener(() => YesButtonClicked?.Invoke());
            noButton.onClick.AddListener(() => NoButtonClicked?.Invoke());
        }

        protected override void OnClose()
        {
            inputService.Enable = true;
            Destroy(gameObject);
        }

        public PrototypeStats GetNewStats()
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
    }
}
