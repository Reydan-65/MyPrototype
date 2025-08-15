using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class PauseWindow : WindowBase
    {
        public event UnityAction ContinueButtonClicked;
        public event UnityAction SettingsButtonClicked;
        public event UnityAction MainMenuButtonClicked;
        public event UnityAction QuitGameButtonClicked;

        [Header("Difficulty Info")]
        [SerializeField] private TextMeshProUGUI difficultyInfoText;

        [SerializeField] private Button continueButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitGameButton;

        public TextMeshProUGUI DifficultyInfoText => difficultyInfoText;

        private void Start()
        {
            continueButton.onClick.AddListener(() => ContinueButtonClicked?.Invoke());
            settingsButton.onClick.AddListener(() => SettingsButtonClicked?.Invoke());
            mainMenuButton.onClick.AddListener(() => MainMenuButtonClicked?.Invoke());
            quitGameButton.onClick.AddListener(() => QuitGameButtonClicked?.Invoke());
        }

        public void SetDifficultyInfoText(string text) => difficultyInfoText.text = $"DIFFICULTY: {text}";
        protected override void OnClose() => Destroy(gameObject);
    }
}
