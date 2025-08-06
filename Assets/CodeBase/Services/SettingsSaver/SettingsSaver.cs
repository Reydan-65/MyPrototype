using CodeBase.Data;
using CodeBase.Infrastructure.Services.SettingsProvider;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SettingsSaver
{
    public class SettingsSaver : ISettingsSaver
    {
        private const string SettingsKey = "Settings";

        private ISettingsProvider settingsProvider;

        public SettingsSaver(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
            LoadSettings();
        }

        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey(SettingsKey))
            {
                string json = PlayerPrefs.GetString(SettingsKey);
                var loadedSettings = JsonUtility.FromJson<Settings>(json);

                settingsProvider.Settings = loadedSettings;

                Debug.Log("SETTINGS LOADED!");
            }
            else
                settingsProvider.Settings = Settings.GetDefaultSettings();

            settingsProvider.Settings.ApplySettings();
        }

        public void SaveSettings(Settings settings)
        {
            PlayerPrefs.SetString(SettingsKey, JsonUtility.ToJson(settings));
            settings.HasSavedSettings = true;
            settings.WasChanged = false;

            settingsProvider.Settings = settings;

            Debug.Log("SETTINGS SAVED!");
        }

        public Settings GetSettings() => settingsProvider.Settings;
    }
}