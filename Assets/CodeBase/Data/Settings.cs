using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Data
{
    [System.Serializable]
    public class Settings
    {
        public event UnityAction Changed;

        public bool WasChanged;

        public bool HasSavedSettings;
        public bool IsFullscreen;
        public int ResolutionIndex;
        public int GraphicsQuality;
        public float MusicVolume;
        public float SfxVolume;

        public void ResetSettings() => CopyFrom(GetDefaultSettings());
        public void IsChanged() => Changed?.Invoke();

        public static Settings GetDefaultSettings()
        {
            var settings = new Settings();

            settings.HasSavedSettings = false;
            settings.IsFullscreen = true;
            settings.ResolutionIndex = 20;
            settings.GraphicsQuality = 5;
            settings.MusicVolume = 10;
            settings.SfxVolume = 10;

            return settings;
        }

        public void CopyFrom(Settings settings)
        {
            HasSavedSettings = settings.HasSavedSettings;
            IsFullscreen = settings.IsFullscreen;
            ResolutionIndex = settings.ResolutionIndex;
            GraphicsQuality = settings.GraphicsQuality;
            MusicVolume = settings.MusicVolume;
            SfxVolume = settings.SfxVolume;
        }

        public void ApplySettings()
        {
            QualitySettings.SetQualityLevel(GraphicsQuality);
            var resolution = Screen.resolutions[ResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, IsFullscreen);

            // Музыка и звук

            WasChanged = false;
        }

        public void UpdateChangedState(Settings originalSettings)
            => WasChanged = !SettingsAreEqual(this, originalSettings);

        public static bool SettingsAreEqual(Settings a, Settings b)
        {
            return a.IsFullscreen == b.IsFullscreen &&
                   a.ResolutionIndex == b.ResolutionIndex &&
                   a.GraphicsQuality == b.GraphicsQuality &&
                   a.MusicVolume == b.MusicVolume &&
                   a.SfxVolume == b.SfxVolume;
        }
    }
}