using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.PlayerProgressSaver
{
    public interface IProgressBeforeSaveHandler : IService
    {
        void UpdateProgressBeforeSave(PlayerProgress progress);
    }
}
