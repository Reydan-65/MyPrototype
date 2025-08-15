using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.GamePlay.UI.Services
{
    public interface IWindowsProvider : IService
    {
        WindowID SourceWindowID {  get; }
        void Open(WindowID id);
        void SetSourceWindow(WindowID id);
    }
}