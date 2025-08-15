using UnityEngine;

namespace CodeBase.Sounds
{
    public class MusicPlayer : AudioPlayer
    {
        private AudioSource audioSource;

        protected override void Awake()
        {
            AudioSource[] sources = GetComponents<AudioSource>();

            audioSource = sources[0];
            audioSource.loop = true;
            audioSource.playOnAwake = false;

            UpdateAudioVolume();
        }

        public override void UpdateAudioVolume()
        {
            if (audioSource == null || settingsProvider?.Settings == null)
                return;

            audioSource.volume = settingsProvider.Settings.MusicVolume / 10f;
        }

        public void PlayMusic(AudioClip clip)
        {
            if (audioSource == null || clip == null) return;

            audioSource.clip = clip;
            audioSource.Play();
        }

        public void StopMusic()
        {
            if (audioSource != null)
                audioSource.Stop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            audioSource = null;
        }
    }
}
