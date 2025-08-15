using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using System;

namespace CodeBase.Infrastructure.Services.SettingsProvider
{
    public interface ISettingsProvider : IService
    {
        event Action Initialized;
        bool IsInitialized { get; }
        Settings Settings { get; set; }
        void Dispose();
    }
}