using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroPreviewLogic : MonoBehaviour
    {
        [SerializeField] private GameObject maleSkinPreview;
        [SerializeField] private GameObject femaleSkinPreview;

        private IProgressProvider progressProvider;
        private bool initialized;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;
            initialized = true;

            InitializeIfReady();
        }

        private void Start()
        {
            InitializeIfReady();
        }

        private void InitializeIfReady()
        {
            if (!initialized) return;
            if (progressProvider == null) return;

            UpdatePreview();
        }

        public void UpdatePreview()
        {
            if (progressProvider == null || progressProvider.PlayerProgress == null) return;

            var progress = progressProvider.PlayerProgress;
            bool useFemaleSkin = progress.HeroSkinID == HeroSkinID.Female &&
                                progress.PurchaseData.IsFemaleSkinUnlocked;

            if (maleSkinPreview != null) maleSkinPreview.SetActive(!useFemaleSkin);
            if (femaleSkinPreview != null) femaleSkinPreview.SetActive(useFemaleSkin);
        }
    }
}