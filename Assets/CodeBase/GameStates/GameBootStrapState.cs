using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Infrastructure.Services.SettingsSaver;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.GamePlay.UI.Services;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.GameStates
{
    public class GameBootStrapState : IEnterableState, IService
    {
        private IGameStateSwitcher gameStateSwitcher;
        private IProgressSaver progressSaver;
        private ISettingsSaver settingsSaver;
        private IConfigsProvider configProvider;
        private IUIFactory uiFactory;

        public GameBootStrapState(
            IGameStateSwitcher gameStateSwitcher,
            IProgressSaver progressSaver,
            ISettingsSaver settingsSaver,
            IConfigsProvider configProvider,
            IUIFactory uiFactory)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressSaver = progressSaver;
            this.settingsSaver = settingsSaver;
            this.configProvider = configProvider;
            this.uiFactory = uiFactory;
        }

        public async void EnterAsync()
        {
            await Unity.Services.Core.UnityServices.InitializeAsync();

            configProvider.Load();

            await uiFactory.WarmUp();

            settingsSaver.LoadSettings();

            //string json = PlayerPrefs.GetString("Settings");
            //Debug.Log(json);

            progressSaver.LoadProgress();

            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;

            Addressables.InitializeAsync();

            if (SceneManager.GetActiveScene().name == Constants.BootStrapSceneName ||
                SceneManager.GetActiveScene().name == Constants.MainMenuSceneName)
            {
                gameStateSwitcher.Enter<LoadMainMenuState>();
            }
        }
    }
}