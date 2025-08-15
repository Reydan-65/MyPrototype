using CodeBase.Data;
using System;

namespace CodeBase.Infrastructure.Services.SettingsProvider
{
    public class SettingsProvider : ISettingsProvider, IDisposable
    {
        public event Action Initialized;

        private bool isInitialized;
        public bool IsInitialized => isInitialized;

        private Settings settings;
        public Settings Settings
        {
            get => settings;
            set
            {
                if (value == null) return;

                if (!isInitialized)
                    Initialized?.Invoke();

                settings = value;
                settings?.IsChanged();
            }
        }

        public void Dispose() => Settings = null;
    }
}