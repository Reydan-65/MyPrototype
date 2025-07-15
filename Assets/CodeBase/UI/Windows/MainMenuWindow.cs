using CodeBase.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public class MainMenuWindow : WindowBase
    {
        public event UnityAction PlayButtonClicked;
        //public event UnityAction ShopButtonClicked;
        public event UnityAction ResetButtonClicked;

        public event UnityAction SelectMaleSkinButtonClicked;
        public event UnityAction SelectFemaleSkinButtonClicked;

        [SerializeField] private Button playButton;
        //[SerializeField] private Button shopButton;
        [SerializeField] private Button resetButton;

        [SerializeField] private Button selectMaleSkinButton;
        [SerializeField] private Button selectFemaleSkinButton;
        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Sprite unselectedButtonSprite;

        [SerializeField] private TextMeshProUGUI levelIndex;

        private void Start()
        {
            playButton.onClick.AddListener(() => PlayButtonClicked?.Invoke());
            //shopButton.onClick.AddListener(() => ShopButtonClicked?.Invoke());
            resetButton.onClick.AddListener(() => ResetButtonClicked?.Invoke());

            selectMaleSkinButton.onClick.AddListener(() => SelectMaleSkinButtonClicked?.Invoke());
            selectFemaleSkinButton.onClick.AddListener(() => SelectFemaleSkinButtonClicked?.Invoke());
        }

        // Level
        public void SetLevelIndex(int index)
        {
            levelIndex.text = Constants.PlayButtonPrefix + (index + 1).ToString();
        }

        public void HideLevelButton()
        {
            playButton.gameObject.SetActive(false);
        }

        // Hero Skin
        public void SetSkinSelectionVisibility(bool isVisible)
        {
            if (selectMaleSkinButton != null && selectFemaleSkinButton != null)
            {
                selectMaleSkinButton.gameObject.SetActive(isVisible);
                selectFemaleSkinButton.gameObject.SetActive(isVisible);
            }
        }

        public void UpdateSkinSelectionButtonsView(HeroSkinID selectedSkin)
        {
            selectMaleSkinButton.image.sprite = selectedSkin == HeroSkinID.Male ? selectedButtonSprite : unselectedButtonSprite;
            selectFemaleSkinButton.image.sprite = selectedSkin == HeroSkinID.Female ? selectedButtonSprite : unselectedButtonSprite;
        }

        protected override void OnClose()
        {
            Destroy(gameObject);
        }
    }
}
