using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CodeBase.GamePlay.UI
{
    public class LevelResultWindow : WindowBase
    {
        public event UnityAction MenuButtonClicked;

        [SerializeField] private Button menuButton;

        private void Start()
        {
            menuButton.onClick.AddListener(() => MenuButtonClicked?.Invoke());
        }
    }
}
