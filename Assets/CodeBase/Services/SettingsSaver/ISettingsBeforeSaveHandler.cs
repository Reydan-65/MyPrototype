using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.SettingsSaver
{
    public interface ISettingsBeforeSaveHandler : IService
    {
        void UpdateSettingsBeforeSave(Settings settings);
    }
}
