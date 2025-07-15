using CodeBase.Configs;
using CodeBase.Data;
using CodeBase.GamePlay;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelResearchState : LevelBaseState, IEnterableState, ITickableState, IExitableState
    {
        private int activePersuersCount;
        private IConfigsProvider configProvider;

        private LevelConfig levelConfig;
        private IProgressProvider progressProvider;
        private HeroInventoryData inventoryData;

        public LevelResearchState(
            IGameFactory gameFactory,
            ILevelStateSwitcher levelStateSwitcher,
            IConfigsProvider configProvider,
            IProgressProvider progressProvider)
        : base(gameFactory, levelStateSwitcher)
        {
            this.configProvider = configProvider;
            this.progressProvider = progressProvider;
        }

        public void Enter()
        {
            activePersuersCount = 0;

            levelConfig = configProvider.GetLevelConfig(SceneManager.GetActiveScene().name);
            inventoryData = progressProvider.PlayerProgress.HeroInventoryData;

            gameFactory.HeroHealth.Die += OnHeroDie;

            SubscribeToEnemyEvents(persuer =>
            {
                persuer.PersuitTarget += OnPersuitTarget;
            });

            Debug.Log("LEVEL: Research state");
        }

        public void Tick()
        {
            CheckVictory();
        }

        public void Exit()
        {
            gameFactory.HeroHealth.Die -= OnHeroDie;

            SubscribeToEnemyEvents(persuer =>
            {
                persuer.PersuitTarget -= OnPersuitTarget;
            });
        }

        private void OnHeroDie()
        {
            levelStateSwitcher.Enter<LevelLostState>();
        }

        private void CheckVictory()
        {
            if (Vector3.Distance(gameFactory.HeroObject.transform.position, levelConfig.FinishPoint) < FinishPoint.Radius
                                 && inventoryData.HasKey)
            {
                levelStateSwitcher.Enter<LevelVictoryState>();
                inventoryData.HasKey = false;
            }
        }

        private void OnPersuitTarget()
        {
            activePersuersCount++;

            levelStateSwitcher.Enter<LevelBattleState>();
        }
    }
}
