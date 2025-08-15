using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Sounds;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class Platform : SavableInteractable
    {
        public SFXEvent PlaySFX;

        [SerializeField] private Animator platformAnimator;
        [SerializeField] private string uniqueID = "platform_";

        private bool isActivated;

        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(IAssetProvider assetProvider) => this.assetProvider = assetProvider;

        public override bool IsActivated
        {
            get => isActivated;
            set
            {
                if (isActivated != value)
                {
                    isActivated = value;
                    if (isActivated) ActivatedVisual();
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            SFXPlayer sfxPlayer = GetComponent<SFXPlayer>();
            sfxPlayer?.UpdateAudioVolume();
        }

        private void ActivatedVisual()
        {
            if (platformAnimator != null)
                platformAnimator.enabled = true;
        }

        public override async void Interact()
        {
            PlaySFX?.Invoke(await assetProvider.Load<AudioClip>(AssetAddress.PlatformIsActiveSound), 0.5f, 1f,1f);

            base.Interact();
            if (isActivated) return;
            isActivated = true;
            ActivatedVisual();
            progressSaver.SaveProgress();
        }

        public override string UniqueID => uniqueID;

        public override void LoadProgress(PlayerProgress progress)
        {
            if (progress == null) return;

            if (progress.TryGetInteractiveState(UniqueID, out bool state))
            {
                isActivated = state;
                if (isActivated) ActivatedVisual();
            }
        }

        public override void UpdateProgressBeforeSave(PlayerProgress progress)
            => progress.SetInteractiveState(UniqueID, IsActivated);
    }
}
