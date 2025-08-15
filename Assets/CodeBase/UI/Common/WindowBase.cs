using CodeBase.Sounds;
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

        private void Awake()
        {
            OnAwake();
            closeButton?.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            closeButton?.onClick.RemoveListener(Close);
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

        public void SetSFXPlayer(SFXPlayer sfxPlayer) => this.sfxPlayer = sfxPlayer;
    }
}
