using CodeBase.Data;
using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.SettingsProvider;
using CodeBase.Infrastructure.Services.SettingsSaver;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class SettingsItemContainer : MonoBehaviour
    {
        [SerializeField] private Transform parent;

        private Settings currentSettings;
        private Settings newSettings;
        private List<SettingsItem> items = new List<SettingsItem>();

        public Settings CurrentSettings => currentSettings;
        public Settings NewSettings => newSettings;

        private bool settingsInitialized = false;
        private bool isInitialized;

        private IUIFactory uiFactory;
        private ISettingsSaver settingsSaver;
        private ISettingsProvider settingsProvider;

        [Inject]
        public void Construct(
            IUIFactory uiFactory,
            ISettingsSaver settingsSaver,
            ISettingsProvider settingsProvider)
        {
            this.uiFactory = uiFactory;
            this.settingsSaver = settingsSaver;
            this.settingsProvider = settingsProvider;
        }

        public void Initialize()
        {
            if (isInitialized) return;

            isInitialized = true;

            if (settingsProvider?.Settings != null)
            {
                settingsProvider.Settings.Changed -= UpdateElements;
                settingsProvider.Settings.Changed += UpdateElements;
            }

            UpdateElements();
            InitializeSettings();
            UpdateAllItemsDisplay(currentSettings);
        }

        private void OnDestroy()
        {
            if (settingsProvider?.Settings != null)
                settingsProvider.Settings.Changed -= UpdateElements;
        }

        public void UpdateElements()
        {
            ClearContainer();
            CreateSettingsItemsAsync();
        }

        private void InitializeSettings()
        {
            if (settingsInitialized) return;

            var savedSettings = settingsSaver.GetSettings();

            if (savedSettings == null)
                savedSettings = Settings.GetDefaultSettings();

            currentSettings = new Settings();
            currentSettings.CopyFrom(savedSettings);

            newSettings = new Settings();
            newSettings.CopyFrom(savedSettings);

            settingsInitialized = true;
        }

        private async void CreateSettingsItemsAsync()
        {
            Settings settings = newSettings ?? settingsSaver.GetSettings();

            foreach (SettingItemID id in Enum.GetValues(typeof(SettingItemID)))
            {
                SettingsItem item = await uiFactory.CreateSettingsItemAsync();
                item.transform.SetParent(parent, false);
                items.Add(item);

                InitializeItem(item, id, settings);
                UpdateItemDisplay(item, id, settings);
            }
        }

        private void InitializeItem(SettingsItem item, SettingItemID id, Settings settings)
        {
            item.Initialize(id, GetTitleForSetting(id));

            Slider slider = item.GetSettingSlider();

            if (slider != null)
            {
                ConfigureSlider(slider, id, settings);
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener((value) => OnSliderValueChanged(id, value));
            }
        }

        private void ConfigureSlider(Slider slider, SettingItemID id, Settings settings)
        {
            switch (id)
            {
                case SettingItemID.FullScreen:
                    slider.minValue = 0;
                    slider.maxValue = 1;
                    slider.wholeNumbers = true;
                    slider.value = settings.IsFullscreen ? 1 : 0;
                    break;
                case SettingItemID.Resolution:
                    slider.minValue = 0;
                    slider.maxValue = settings.SupportedResolutions.Count - 1;
                    slider.wholeNumbers = true;
                    slider.value = settings.ResolutionIndex;
                    break;
                case SettingItemID.GraphicsQuality:
                    slider.minValue = 0;
                    slider.maxValue = QualitySettings.names.Length - 1;
                    slider.wholeNumbers = true;
                    slider.value = settings.GraphicsQuality;
                    break;
                case SettingItemID.MusicVolume:
                case SettingItemID.SFXVolume:
                    slider.minValue = 0;
                    slider.maxValue = 10;
                    slider.wholeNumbers = true;
                    slider.value = Mathf.RoundToInt(GetSettingValue(id, settings) * 10);
                    break;
            }
        }

        private void OnSliderValueChanged(SettingItemID id, float value)
        {
            if (currentSettings == null || newSettings == null) return;

            switch (id)
            {
                case SettingItemID.FullScreen:
                    newSettings.IsFullscreen = value > 0.5f;
                    newSettings.ApplySettings();
                    break;
                case SettingItemID.Resolution:
                    newSettings.ResolutionIndex = Mathf.RoundToInt(value);
                    break;
                case SettingItemID.GraphicsQuality:
                    newSettings.GraphicsQuality = Mathf.RoundToInt(value);
                    newSettings.ApplySettings();
                    break;
                case SettingItemID.MusicVolume:
                    newSettings.MusicVolume = value;
                    newSettings.ApplySettings();
                    break;
                case SettingItemID.SFXVolume:
                    newSettings.SfxVolume = value;
                    newSettings.ApplySettings();
                    break;
            }

            newSettings.UpdateChangedState(currentSettings);
            newSettings.IsChanged();

            UpdateAllItemsDisplay(newSettings);
        }

        

        private void UpdateAllItemsDisplay(Settings settings)
        {
            foreach (var item in items)
            {
                if (item == null || item.gameObject == null) continue;

                var slider = item.GetSettingSlider();
                if (slider != null)
                    slider.SetValueWithoutNotify(GetSettingValue(item.ID, settings));

                UpdateItemDisplay(item, item.ID, settings);
            }
        }

        private void UpdateItemDisplay(SettingsItem item, SettingItemID id, Settings settings)
        {
            float value = GetSettingValue(id, settings);

            switch (id)
            {
                case SettingItemID.FullScreen:
                    item.UpdateCurrentValue(settings.IsFullscreen ? "ON" : "OFF");
                    break;
                case SettingItemID.Resolution:
                    int index = Mathf.Clamp(Mathf.RoundToInt(GetSettingValue(id, settings)), 0, settings.SupportedResolutions.Count - 1);
                    var res = settings.SupportedResolutions[index];
                    item.UpdateCurrentValue($"{res.x}x{res.y}");
                    break;
                case SettingItemID.GraphicsQuality:
                    item.UpdateCurrentValue(QualitySettings.names[settings.GraphicsQuality]);
                    break;
                case SettingItemID.MusicVolume:
                case SettingItemID.SFXVolume:
                    item.UpdateCurrentValue($"{value * 10}%");
                    break;
            }
        }

        private float GetSettingValue(SettingItemID id, Settings settings)
        {
            return id switch
            {
                SettingItemID.FullScreen => settings.IsFullscreen ? 1 : 0,
                SettingItemID.Resolution => settings.ResolutionIndex,
                SettingItemID.GraphicsQuality => settings.GraphicsQuality,
                SettingItemID.MusicVolume => settings.MusicVolume,
                SettingItemID.SFXVolume => settings.SfxVolume,
                _ => 0
            };
        }

        private string GetTitleForSetting(SettingItemID id)
        {
            return id switch
            {
                SettingItemID.FullScreen => "FULL SCREEN",
                SettingItemID.Resolution => "RESOLUTION",
                SettingItemID.GraphicsQuality => "GRAPHICS QUALITY",
                SettingItemID.MusicVolume => "MUSIC VOLUME",
                SettingItemID.SFXVolume => "SFX VOLUME",
                _ => string.Empty
            };
        }

        private void ClearContainer()
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);
            items.Clear();
        }

        public void ApplyChanges()
        {
            if (currentSettings == null || newSettings == null) return;

            currentSettings.CopyFrom(newSettings);
            currentSettings.ApplySettings();
            settingsSaver.SaveSettings(currentSettings);
        }

        public void ResetToDefaults()
        {
            if (newSettings == null) return;

            var defaultSettings = Settings.GetDefaultSettings();
            newSettings.CopyFrom(defaultSettings);
            newSettings.WasChanged = !Settings.SettingsAreEqual(newSettings, currentSettings);
            newSettings.ApplySettings();

            UpdateAllItemsDisplay(newSettings);
        }

        public void RevertChanges()
        {
            if (newSettings == null || currentSettings == null) return;

            newSettings.CopyFrom(currentSettings);
            settingsSaver.SaveSettings(newSettings);
            newSettings.ApplySettings();
            newSettings.WasChanged = false;

            UpdateAllItemsDisplay(newSettings);
        }
    }
}
