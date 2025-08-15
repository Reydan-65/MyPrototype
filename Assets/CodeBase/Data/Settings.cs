using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Data
{
    [System.Serializable]
    public class Settings
    {
        public event UnityAction Changed;

        public List<Vector2Int> SupportedResolutions = new List<Vector2Int>
        {
            new Vector2Int(800, 600),
            new Vector2Int(1024, 768),
            new Vector2Int(1280, 720),
            new Vector2Int(1920, 1080),
            new Vector2Int(2560, 1440)
        };

        public bool WasChanged;

        public bool HasSavedSettings;
        public bool IsFullscreen;
        public int ResolutionIndex;
        public int GraphicsQuality;
        public float MusicVolume;
        public float SFXVolume;

        public void ResetSettings() => CopyFrom(GetDefaultSettings());
        public void IsChanged() => Changed?.Invoke();

        public static Settings GetDefaultSettings()
        {
            var settings = new Settings();

            settings.HasSavedSettings = false;
            settings.IsFullscreen = true;
            settings.ResolutionIndex = 3;
            settings.GraphicsQuality = 5;
            settings.MusicVolume = 10;
            settings.SFXVolume = 10;

            return settings;
        }

        public void CopyFrom(Settings settings)
        {
            HasSavedSettings = settings.HasSavedSettings;
            IsFullscreen = settings.IsFullscreen;
            ResolutionIndex = settings.ResolutionIndex;
            GraphicsQuality = settings.GraphicsQuality;
            MusicVolume = settings.MusicVolume;
            SFXVolume = settings.SFXVolume;
        }

        public void ApplySettings()
        {
            QualitySettings.SetQualityLevel(GraphicsQuality);

            if (ResolutionIndex >= 0 && ResolutionIndex < SupportedResolutions.Count)
            {
                var res = SupportedResolutions[ResolutionIndex];
                Screen.SetResolution(res.x, res.y, IsFullscreen);
            }
            else
                Screen.SetResolution(2560, 1440, IsFullscreen);

            // Музыка и звук иначе

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
                   a.SFXVolume == b.SFXVolume;
        }
    }
}