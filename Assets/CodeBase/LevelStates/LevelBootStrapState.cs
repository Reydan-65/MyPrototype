using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.GamePlay.Enemies;
using CodeBase.Configs;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelBootStrapState : IEnterableState, IService
    {
        private IGameFactory gameFactory;
        private ILevelStateSwitcher levelStateSwitcher;
        private IInputService inputService;
        private ICursorService cursorService;
        private IProgressSaver progressSaver;
        private IConfigsProvider configProvider;

        public LevelBootStrapState(
            IGameFactory gameFactory,
            ILevelStateSwitcher levelStateSwitcher,
            IInputService inputService,
            ICursorService cursorService,
            IProgressSaver progressSaver,
            IConfigsProvider configProvider)
        {
            this.gameFactory = gameFactory;
            this.levelStateSwitcher = levelStateSwitcher;
            this.inputService = inputService;
            this.cursorService = cursorService;
            this.progressSaver = progressSaver;
            this.configProvider = configProvider;
        }

        public async void Enter()
        {
            //Debug.Log("LEVEL: Init");

            progressSaver.ClearObjects();

            await gameFactory.WarmUp();

            string sceneName = SceneManager.GetActiveScene().name;
            LevelConfig levelConfig = configProvider.GetLevelConfig(sceneName);

            EnemySpawner[] enemySpawners = GameObject.FindObjectsByType<EnemySpawner>(0);

            for (int i = 0; i < enemySpawners.Length; i++)
            {
                enemySpawners[i].Spawn();
            }

            await gameFactory.CreateHeroAsync(levelConfig.HeroSpawnPoint, Quaternion.identity);

            FollowCamera followCamera = await gameFactory.CreateFollowCameraAsync();
            followCamera.SetTarget(gameFactory.HeroObject.transform);

            cursorService.SetCamera(followCamera.GetComponentInChildren<Camera>());

            //await gameFactory.CreateJoystickAsync();

            progressSaver.LoadProgress();

            inputService.Enable = true;

            levelStateSwitcher.Enter<LevelResearchState>();
        }
    }
}
