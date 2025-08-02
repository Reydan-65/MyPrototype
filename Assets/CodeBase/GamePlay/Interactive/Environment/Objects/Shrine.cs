using CodeBase.Infrastructure.Services;
using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine;
using CodeBase.Infrastructure.Services.Factory;

namespace CodeBase.GamePlay.Interactive
{
    public class Shrine : Interactable, IInputInteractable
    {
        private IWindowsProvider windowsProvider;
        private IGameFactory gameFactory;
        private IEnemySpawnManager enemySpawnManager;

        [Inject]
        public void Construct(
            IWindowsProvider windowsProvider,
            IGameFactory gameFactory,
            IEnemySpawnManager enemySpawnManager)
        {
            this.windowsProvider = windowsProvider;
            this.gameFactory = gameFactory;
            this.enemySpawnManager = enemySpawnManager;
        }

        public override void Interact()
        {
            base.Interact();

            enemySpawnManager.DestroyAllEnemies();
            enemySpawnManager.SpawnAllEnemies();

            ObjectsDestroyer.DestroyObjectsByTag("ToDestroy");

            windowsProvider.Open(WindowID.ShrineWindow);
        }

        protected override void OnHUDWindowCreated()
        {
            base.OnHUDWindowCreated();

            if (interactableTracker != null)
                interactableTracker.RegisterInteractable(this);
        }
    }
}