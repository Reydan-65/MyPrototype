using CodeBase.Infrastructure.Services;
using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class Shrine : Interactable
    {
        private IWindowsProvider windowsProvider;
        private IEnemySpawnManager enemySpawnManager;

        [Inject]
        public void Construct(
            IWindowsProvider windowsProvider,
            IEnemySpawnManager enemySpawnManager)
        {
            this.windowsProvider = windowsProvider;
            this.enemySpawnManager = enemySpawnManager;
        }

        public override void Interact(GameObject user)
        {
            base.Interact(user);

            enemySpawnManager.DestroyAllEnemies();
            enemySpawnManager.SpawnAllEnemies();

            ObjectsDestroyer.DestroyObjectsByTag("ToDestroy");

            windowsProvider.Open(WindowID.ShrineWindow);
        }
    }
}