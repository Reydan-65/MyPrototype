using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Sounds;
using CodeBase.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
        protected SFXPlayer sfxPlayer;
        public SFXPlayer SFXPlayer => sfxPlayer;

        public event UnityAction CleanUped;
        public event UnityAction Closed;

        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI titleText;

        private IGameFactory gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
            if (this.gameFactory != null)
                this.gameFactory.AudioPlayerCreated += OnAudioPlayerCreated;
        }

        private void Awake()
        {
            OnAwake();
            closeButton?.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            closeButton?.onClick.RemoveListener(Close);
            if (gameFactory != null)
                gameFactory.AudioPlayerCreated -= OnAudioPlayerCreated;
            OnCleanUp();
            CleanUped?.Invoke();
        }

        public void Close()
        {
            Closed?.Invoke();
            OnClose();
        }

        public void SetTitle(string title)
        {
            if (titleText == null) return;

            titleText.text = title;
        }

        protected virtual void OnAwake() { }
        protected virtual void OnClose() { }
        protected virtual void OnCleanUp() { }

        public void SetConfirmPanelState(GameObject firstPanel, GameObject secondPanel, bool isActive)
        {
            if (firstPanel == null || secondPanel == null) return;

            firstPanel.SetActive(!isActive);
            secondPanel.SetActive(isActive);
        }

        private void OnAudioPlayerCreated()
        {
            UIClickSound[] clickSounds = GetComponentsInChildren<UIClickSound>();
            if (clickSounds != null && clickSounds.Length > 0)
                foreach (UIClickSound clickSound in clickSounds)
                    clickSound.SetWindow(this);

            sfxPlayer = gameFactory.AudioPlayer.GetComponent<SFXPlayer>();
        }

        public void SetSFXPlayer()
        {
            if (gameFactory?.AudioPlayer != null)
                sfxPlayer = gameFactory.AudioPlayer.GetComponent<SFXPlayer>();
        }
    }
}
