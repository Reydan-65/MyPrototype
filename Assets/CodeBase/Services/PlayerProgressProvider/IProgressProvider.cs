using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services.PlayerProgressProvider
{
    public interface IProgressProvider : IService
    {
        PlayerProgress PlayerProgress { get; set; }
    }
}