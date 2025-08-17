using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class MainMenuWindow : WindowBase
    {
        public event UnityAction ContinueButtonClicked;
        public event UnityAction NewGameButtonClicked;
        public event UnityAction SettingsButtonClicked;
        public event UnityAction QuitGameButtonClicked;
        public event UnityAction ResetGameButtonClicked;

        public event UnityAction YesButtonClicked;
        public event UnityAction NoButtonClicked;

        [Header("Panels")]
        [SerializeField] private GameObject containerPanel;
        [SerializeField] private GameObject confirmPanelPanel;
        public GameObject ContainerPanel => containerPanel;
        public GameObject ConfirmPanel => confirmPanelPanel;

        [Header("Difficulty Info")]
        [SerializeField] private TextMeshProUGUI difficultyInfoText;
        public TextMeshProUGUI DifficultyInfoText => difficultyInfoText;

        [Header("Container Panel")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitGameButton;
        [SerializeField] private Button resetGameButton;

        [Header("Warning Panel")]
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        private void Start()
        {
            continueButton.onClick.AddListener(() => ContinueButtonClicked?.Invoke());
            newGameButton.onClick.AddListener(() => NewGameButtonClicked?.Invoke());
            settingsButton.onClick.AddListener(() => SettingsButtonClicked?.Invoke());
            quitGameButton.onClick.AddListener(() => QuitGameButtonClicked?.Invoke());
            resetGameButton.onClick.AddListener(() => ResetGameButtonClicked?.Invoke());

            yesButton.onClick.AddListener(() => YesButtonClicked?.Invoke());
            noButton.onClick.AddListener(() => NoButtonClicked?.Invoke());
        }

        public void SetContinueButtonState(bool isActive) => continueButton.interactable = isActive;
        public void SetDifficultyInfoText(string text) => difficultyInfoText.text = $"DIFFICULTY: {text}";
        protected override void OnClose() => Destroy(gameObject);
    }
}
