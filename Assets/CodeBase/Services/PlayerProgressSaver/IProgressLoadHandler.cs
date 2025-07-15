using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.PlayerProgressSaver
{
    public interface IProgressLoadHandler : IService
    {
        void LoadProgress(PlayerProgress progress);
    }
}
