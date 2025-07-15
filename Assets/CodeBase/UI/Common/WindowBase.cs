using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI
{
    public abstract class WindowBase : MonoBehaviour
    {
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
    }
}
