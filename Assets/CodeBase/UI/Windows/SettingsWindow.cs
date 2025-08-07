using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class SettingsWindow : WindowBase
    {
        public UnityAction DefaultButtonClicked;
        public UnityAction AcceptButtonClicked;
        public UnityAction AdvancedCloseButtonClicked;
        public UnityAction YesButtonClicked;
        public UnityAction NoButtonClicked;

        [Header("Panels")]
        [SerializeField] private GameObject mainBottomPanel;
        [SerializeField] private GameObject confirmBottomPanel;

        [Header("Main Panel Buttons")]
        [SerializeField] private Button defaultButton;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button advancedCloseButton;

        [Header("Confirm Panel Buttons")]
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private SettingsPresenter presenter;
        private SettingsItemContainer container;

        public GameObject MainBottomPanel => mainBottomPanel;
        public GameObject ConfirmBottomPanel => confirmBottomPanel;
        public Button DefaultButton => defaultButton;
        public Button AcceptButton => acceptButton;

        public SettingsItemContainer Container => container;

        public void InitializeContainer(SettingsPresenter presenter)
        {
            this.presenter = presenter;

            container = GetComponent<SettingsItemContainer>();

            if (container != null) container.Initialize();

            acceptButton.onClick.AddListener(() => AcceptButtonClicked?.Invoke());
            defaultButton.onClick.AddListener(() => DefaultButtonClicked?.Invoke());
            advancedCloseButton.onClick.AddListener(() => AdvancedCloseButtonClicked?.Invoke());
            yesButton.onClick.AddListener(() => YesButtonClicked?.Invoke());
            noButton.onClick.AddListener(() => NoButtonClicked?.Invoke());

            SetConfirmPanelState(mainBottomPanel, confirmBottomPanel, false);
        }

        public SettingsPresenter GetPresenter() => presenter;
        protected override void OnClose() => Destroy(gameObject);
    }
}
