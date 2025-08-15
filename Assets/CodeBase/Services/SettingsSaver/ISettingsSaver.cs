using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.SettingsSaver
{
    public interface ISettingsSaver : IService
    {
        void LoadSettings();
        void SaveSettings(Settings settings);
        Settings GetSettings();
    }
}