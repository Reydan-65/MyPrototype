using CodeBase.GamePlay.UI;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class UIClickSound : MonoBehaviour
    {
        private AudioClip cachedClickSound;
        private Button button;
        private WindowBase window;
        private CancellationTokenSource cts;
        private bool isSoundLoaded;

        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(IAssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
            PreloadSound();
        }

        private void Awake()
        {
            cts = new CancellationTokenSource();
            button = GetComponent<Button>();
        }

        private async void PreloadSound()
        {
            cachedClickSound = await assetProvider.Load<AudioClip>(AssetAddress.ClickButtonSound);
            isSoundLoaded = true;
            if (button != null)
                button.onClick.AddListener(PlaySound);
        }

        public void SetWindow(WindowBase window)
        {
            this.window = window;
        }    

        public void PlaySound()
        {
            if (!CanPlaySound()) return;

            var audioSources = window.SFXPlayer.GetComponents<AudioSource>();
            if (audioSources.Length > 1)
                audioSources[1].PlayOneShot(cachedClickSound);
        }

        private bool CanPlaySound()
        {
            if (window == null || window.SFXPlayer == null) return false;
            if (!isSoundLoaded || cachedClickSound == null) return false;

            return true;
        }

        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
            if (button != null)
                button.onClick.RemoveListener(PlaySound);
        }
    }
}
