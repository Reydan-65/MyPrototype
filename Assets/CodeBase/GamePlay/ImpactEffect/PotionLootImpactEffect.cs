using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Sounds;
using UnityEngine;

namespace CodeBase.GamePlay
{
    public class PotionLootImpactEffect : MonoBehaviour
    {
        public SFXEvent PlaySFX;

        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(IAssetProvider assetProvider) => this.assetProvider = assetProvider;

        private async void Start()
            => PlaySFX?.Invoke(await assetProvider.Load<AudioClip>(AssetAddress.PotionPickupItemSound), 0.5f, 0.95f, 1.05f);
    }
}
