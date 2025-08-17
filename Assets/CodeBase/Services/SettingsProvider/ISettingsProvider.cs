using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using System;

namespace CodeBase.Infrastructure.Services.SettingsProvider
{
    public interface ISettingsProvider :IService
    {
        bool IsInitialized { get; }
        Settings Settings { get; set; }

        event Action Initialized;

        void Dispose();
    }
}