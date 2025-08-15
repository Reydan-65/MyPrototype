using CodeBase.GamePlay.Enemies;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine.Events;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public abstract class LevelBaseState : IService
    {
        protected IGameFactory gameFactory;
        protected ILevelStateSwitcher levelStateSwitcher;

        protected LevelBaseState(IGameFactory gameFactory, ILevelStateSwitcher levelStateSwitcher)
        {
            this.gameFactory = gameFactory;
            this.levelStateSwitcher = levelStateSwitcher;
        }

        protected void SubscribeToEnemyEvents(UnityAction<EnemyTargetPersuer> subscribeAction)
        {
            foreach (var enemy in gameFactory.EnemiesObject)
            {
                if (enemy == null) continue;

                EnemyTargetPersuer persuer = enemy.GetComponent<EnemyTargetPersuer>();

                if (persuer != null)
                    subscribeAction(persuer);
            }
        }

        protected void CleanEnemiesList() => gameFactory.EnemiesObject.RemoveAll(enemy => enemy == null);
    }
}
