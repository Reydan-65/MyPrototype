using CodeBase.Infrastructure.DependencyInjection;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public interface IEnemySpawnManager : IService
    {
        void DestroyAllEnemies();
        void RegisterEnemy(GameObject enemy);
        void SpawnAllEnemies();
        void UnregisterEnemy(GameObject enemy);
    }
}