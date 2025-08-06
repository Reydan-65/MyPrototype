using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.SettingsProvider
{
    public interface ISettingsProvider : IService
    {
        Settings Settings { get; set; }
        void Dispose();
    }
}