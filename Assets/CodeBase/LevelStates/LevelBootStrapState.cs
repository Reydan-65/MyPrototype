using CodeBase.Configs;
using CodeBase.GamePlay.Spawners;
using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelBootStrapState : IEnterableState, IService
    {
        private IGameFactory gameFactory;
        private ILevelStateSwitcher levelStateSwitcher;
        private IInputService inputService;
        private ICursorService cursorService;
        private IProgressSaver progressSaver;
        private IConfigsProvider configsProvider;
        private IWindowsProvider windowsProvider;
        private ILootService lootService;
        private IAssetProvider assetProvider;

        public LevelBootStrapState(
            IGameFactory gameFactory,
            ILevelStateSwitcher levelStateSwitcher,
            IInputService inputService,
            ICursorService cursorService,
            IProgressSaver progressSaver,
            IConfigsProvider configsProvider,
            IWindowsProvider windowsProvider,
            ILootService lootService,
            IAssetProvider assetProvider)
        {
            this.gameFactory = gameFactory;
            this.levelStateSwitcher = levelStateSwitcher;
            this.inputService = inputService;
            this.cursorService = cursorService;
            this.progressSaver = progressSaver;
            this.configsProvider = configsProvider;
            this.windowsProvider = windowsProvider;
            this.lootService = lootService;
            this.assetProvider = assetProvider;
        }

        public async void EnterAsync()
        {
            progressSaver.ClearObjects();

            await gameFactory.WarmUp();

            SpawnEnemies();

            await SpawnPrototype();

            FollowCamera followCamera = await gameFactory.CreateFollowCameraAsync();
            followCamera.SetTarget(gameFactory.PrototypeObject.transform);

            cursorService.SetCamera(followCamera.GetComponentInChildren<Camera>());
            progressSaver.LoadProgress();
            lootService.CleanUpPickedLoot();
            windowsProvider.Open(WindowID.HUDWindow);
            inputService.Enable = true;

            AudioPlayer audioPlayer = await gameFactory.CreateAudioPlayerAsync();
            if (audioPlayer != null)
            {
                MusicPlayer musicPlayer = audioPlayer.GetComponent<MusicPlayer>();
                SFXPlayer sfxPlayer = audioPlayer.GetComponent<SFXPlayer>();

                musicPlayer?.UpdateAudioVolume();
                sfxPlayer?.UpdateAudioVolume();

                musicPlayer?.PlayMusic(await assetProvider.Load<AudioClip>(AssetAddress.GameplayMusic));
            }

            levelStateSwitcher.Enter<LevelResearchState>();
        }

        private static async void SpawnEnemies()
        {
            EnemySpawner[] enemySpawners = GameObject.FindObjectsByType<EnemySpawner>(0);

            for (int i = 0; i < enemySpawners.Length; i++)
                await enemySpawners[i].Spawn();
        }

        private async System.Threading.Tasks.Task SpawnPrototype()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            LevelConfig levelConfig = configsProvider.GetLevelConfig(sceneName);
            await gameFactory.CreatePrototypeAsync(levelConfig.PrototypeSpawnPoint, Quaternion.identity);
        }
    }
}
