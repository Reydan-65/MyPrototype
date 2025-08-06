using CodeBase.GamePlay;
using CodeBase.Configs;
using CodeBase.GamePlay.Prototype;
using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelRespawnState : LevelBaseState, IEnterableState
    {
        private const float DELAY = 1f;

        private PrototypeHealth prototypeHealth;

        private IConfigsProvider configsProvider;
        private IInputService inputService;
        private IWindowsProvider windowsProvider;
        private ICoroutineRunner coroutineRunner;
        private IProgressSaver progressSaver;
        private IUIFactory uiFactory;
        private IEnemySpawnManager enemySpawnManager;

        public LevelRespawnState(
            IConfigsProvider configsProvider,
            IInputService inputService,
            ICoroutineRunner coroutineRunner,
            IGameFactory gameFactory,
            ILevelStateSwitcher levelStateSwitcher,
            IWindowsProvider windowsProvider,
            IProgressSaver progressSaver,
            IUIFactory uiFactory,
            IEnemySpawnManager enemySpawnManager)
        : base(gameFactory, levelStateSwitcher)
        {
            this.configsProvider = configsProvider;
            this.inputService = inputService;
            this.coroutineRunner = coroutineRunner;
            this.windowsProvider = windowsProvider;
            this.progressSaver = progressSaver;
            this.uiFactory = uiFactory;
            this.enemySpawnManager = enemySpawnManager;
        }

        public async void EnterAsync()
        {
            Debug.Log("LEVEL: Respawn State");

            enemySpawnManager.DestroyAllEnemies();
            enemySpawnManager.SpawnAllEnemies();

            windowsProvider.Open(WindowID.HUDWindow);

            ObjectsDestroyer.DestroyObjectsByTag("ToDestroy");

            string sceneName = SceneManager.GetActiveScene().name;
            LevelConfig levelConfig = configsProvider.GetLevelConfig(sceneName);


            await gameFactory.CreatePrototypeAsync(levelConfig.PrototypeSpawnPoint, Quaternion.identity);
            prototypeHealth = gameFactory.PrototypeObject.GetComponent<PrototypeHealth>();
            prototypeHealth.SetImmune(true);
            prototypeHealth.Initialize(prototypeHealth.Max);

            gameFactory.FollowCamera.SetTarget(gameFactory.PrototypeObject.transform);

            progressSaver.LoadProgress();

            await RecreateHUD();

            coroutineRunner.StartCoroutine(SwitchStateDelay());
        }

        private IEnumerator SwitchStateDelay()
        {
            yield return new WaitForSeconds(DELAY);

            inputService.Enable = true;
            prototypeHealth.SetImmune(false);

            levelStateSwitcher.Enter<LevelResearchState>();
        }

        private async Task RecreateHUD()
        {
            if (uiFactory.HUDWindow != null)
                uiFactory.HUDWindow.Close();

            var config = configsProvider.GetWindowConfig(WindowID.HUDWindow);
            await uiFactory.CreateHUDWindowAsync(config);

            if (uiFactory.HUDPresenter != null && uiFactory.HUDWindow != null)
                uiFactory.HUDPresenter.SetWindow(uiFactory.HUDWindow);
        }
    }
}
