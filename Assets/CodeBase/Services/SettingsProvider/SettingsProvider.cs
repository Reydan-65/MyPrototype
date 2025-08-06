using CodeBase.Data;
using System;

namespace CodeBase.Infrastructure.Services.SettingsProvider
{
    public class SettingsProvider : ISettingsProvider, IDisposable
    {
        private Settings settings;
        public Settings Settings
        {
            get => settings;
            set
            {
                if (value == null) return;

                settings = value;
                settings?.IsChanged();
            }
        }

        public void Dispose()
        {
            Settings = null;
        }
    }
}