using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.GamePlay.UI.Services
{
    public interface IWindowsProvider : IService
    {
        void Open(WindowID id);
    }
}