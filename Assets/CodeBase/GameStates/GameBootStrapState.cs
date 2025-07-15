using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
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
        private IConfigsProvider configProvider;
        private IUIFactory uiFactory;
        //private IAdsService adsService;
        private IIApService iApService;

        public GameBootStrapState(
            IGameStateSwitcher gameStateSwitcher,
            IProgressSaver progressSaver,
            IConfigsProvider configProvider,
            IUIFactory uiFactory,
            /*IAdsService adsService,*/
            IIApService iApService)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.progressSaver = progressSaver;
            this.configProvider = configProvider;
            this.uiFactory = uiFactory;
            //this.adsService = adsService;
            this.iApService = iApService;
        }

        public async void Enter()
        {
            await Unity.Services.Core.UnityServices.InitializeAsync();

            iApService.Initialize();

            //adsService.Initialize();
            //adsService.LoadInterstitial();
            //adsService.LoadRewarded();

            configProvider.Load();

            await uiFactory.WarmUp();

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