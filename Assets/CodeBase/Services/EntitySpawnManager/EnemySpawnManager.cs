using CodeBase.GamePlay.Spawners;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class EnemySpawnManager : IEnemySpawnManager
    {
        private List<GameObject> activeEnemies = new List<GameObject>();

        public void DestroyAllEnemies()
        {
            foreach (var enemy in activeEnemies)
            {
                if (enemy != null)
                    Object.Destroy(enemy);
            }

            activeEnemies.Clear();
        }

        public void SpawnAllEnemies()
        {
            EnemySpawner[] spawners = Object.FindObjectsOfType<EnemySpawner>();

            foreach (var spawner in spawners)
                spawner.Spawn();
        }

        public void RegisterEnemy(GameObject enemy)
        {
            if (enemy != null && !activeEnemies.Contains(enemy))
                activeEnemies.Add(enemy);
        }

        public void UnregisterEnemy(GameObject enemy)
        {
            if (enemy != null && activeEnemies.Contains(enemy))
                activeEnemies.Remove(enemy);
        }
    }
}
