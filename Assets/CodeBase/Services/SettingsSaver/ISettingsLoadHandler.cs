using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.SettingsSaver
{
    public interface ISettingsLoadHandler : IService
    {
        void LoadSettings(Settings settings);
    }
}
