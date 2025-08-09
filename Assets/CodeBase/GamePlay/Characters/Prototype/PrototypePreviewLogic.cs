using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypePreviewLogic : MonoBehaviour
    {
        [SerializeField] private GameObject baseSkinPreview;
        [SerializeField] private GameObject secondSkinPreview;

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
            //bool useSecondSkin = progress.PrototypeSkinID == PrototypeSkinID.SecondSkin &&
            //                    progress.PurchaseData.IsSecondSkinUnlocked;

            if (baseSkinPreview != null) baseSkinPreview.SetActive(true);
            //if (secondSkinPreview != null) secondSkinPreview.SetActive(useSecondSkin);
        }
    }
}