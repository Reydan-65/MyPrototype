using CodeBase.Data;
using CodeBase.Infrastructure.Services.SettingsProvider;
using System.IO;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SettingsSaver
{
    public class SettingsSaver : ISettingsSaver
    {
        private const string SettingsKey = "Settings";
        private string SavePath => Path.Combine(Application.persistentDataPath, "settings_save.json");
        private ISettingsProvider settingsProvider;

        public SettingsSaver(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
            LoadSettings();
        }

        public void LoadSettings()
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
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
            string json = JsonUtility.ToJson(settingsProvider.Settings);
            File.WriteAllText(SavePath, json);

            settings.HasSavedSettings = true;
            settings.WasChanged = false;

            settingsProvider.Settings = settings;

            Debug.Log("SETTINGS SAVED!");
        }

        public Settings GetSettings() => settingsProvider.Settings;
    }
}