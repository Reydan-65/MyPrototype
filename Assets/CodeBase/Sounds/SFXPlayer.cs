using UnityEngine;

namespace CodeBase.Sounds
{
    public class SFXPlayer : AudioPlayer
    {
        private AudioSource audioSource;

        protected override void Awake()
        {
            AudioSource[] sources = GetComponents<AudioSource>();

            if (sources.Length > 1)
                audioSource = sources[1];
            else if (sources.Length == 1)
                audioSource = sources[0];
            else
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = false;

            if (settingsProvider != null)
                Initialize();

            var components = GetComponents<MonoBehaviour>();

            foreach (var component in components)
            {
                if (component == this) continue;

                var eventField = component.GetType().GetField("PlaySFX");
                if (eventField != null && eventField.FieldType == typeof(SFXEvent))
                {
                    var sfxEvent = (SFXEvent)eventField.GetValue(component);
                    sfxEvent.AddListener(OnSFXRequested);
                }
            }

            UpdateAudioVolume();
        }

        public override void UpdateAudioVolume()
        {
            if (audioSource == null || settingsProvider?.Settings == null)
                return;

            audioSource.volume = settingsProvider.Settings.SFXVolume / 10f;
        }

        private void OnSFXRequested(AudioClip clip, float volumeScale = 1f, float minPitch = 1, float maxPitch = 1)
            => PlaySFX(clip, volumeScale, minPitch, maxPitch);

        public void PlaySFX(AudioClip clip, float volumeScale = 1f, float minPitch = 1, float maxPitch = 1)
        {
            if (isDestroyed || audioSource == null || clip == null) return;

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clip, audioSource.volume * volumeScale);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var components = GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component == this) continue;

                var eventField = component.GetType().GetField("PlaySFX");
                if (eventField != null && eventField.FieldType == typeof(SFXEvent))
                {
                    var sfxEvent = (SFXEvent)eventField.GetValue(component);
                    sfxEvent.RemoveListener(OnSFXRequested);
                }
            }

            audioSource = null;
        }
    }
}
