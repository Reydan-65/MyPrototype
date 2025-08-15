using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.SettingsProvider;
using UnityEngine;

namespace CodeBase.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AudioPlayer : MonoBehaviour, IService
    {
        protected bool isDestroyed;

        protected ISettingsProvider settingsProvider;

        protected virtual void Awake() { }

        [Inject]
        public virtual void Construct(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
            if (settingsProvider.IsInitialized)
                Initialize();
            else
                settingsProvider.Initialized += OnSettingProviderInitialized;
        }

        private void OnSettingProviderInitialized() => Initialize();
        private void OnSettingsChanged() => UpdateAudioVolume();

        protected virtual void Initialize()
        {
            if (settingsProvider == null || settingsProvider.Settings == null)
                return;

            settingsProvider.Settings.Changed -= OnSettingsChanged;
            settingsProvider.Settings.Changed += OnSettingsChanged;

            UpdateAudioVolume();
        }

        public virtual void UpdateAudioVolume() { }

        protected virtual void OnDestroy()
        {
            isDestroyed = true;
            if (settingsProvider?.Settings != null)
                settingsProvider.Settings.Changed -= OnSettingsChanged;
        }
    }
}
