using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface ILootService : IService
    {
        Task DropLoot(Vector3 position, LootItemID lootType, int count = 1);
        void CleanUp();
    }
}
