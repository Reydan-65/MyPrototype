using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay
{
    public class KeyChecker : MonoBehaviour
    {
        [SerializeField] private Image infoImage;
        [SerializeField] private Color[] infoColor;

        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;
        }

        private void Start()
        {
            if (progressProvider == null) return;

            progressProvider.PlayerProgress.HeroInventoryData.KeyPickuped -= OnKeyPickuped;
            progressProvider.PlayerProgress.HeroInventoryData.KeyPickuped += OnKeyPickuped;

            UpdateKeyVisual();
        }

        private void OnDestroy()
        {
            progressProvider.PlayerProgress.HeroInventoryData.KeyPickuped -= OnKeyPickuped;
        }

        private void OnKeyPickuped()
        {
            UpdateKeyVisual();
        }

        private void UpdateKeyVisual()
        {
            if (infoImage == null || infoColor == null || infoColor.Length < 2)
                return;

            infoImage.color = progressProvider.PlayerProgress.HeroInventoryData.HasKey ? infoColor[1] : infoColor[0];
        }
    }
}