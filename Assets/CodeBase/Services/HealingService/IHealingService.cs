using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure.Services
{
    public interface IHealingService : IService
    {
        bool TryHeal(LootItemID id);
    }
}