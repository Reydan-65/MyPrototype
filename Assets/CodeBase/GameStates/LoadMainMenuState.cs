using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Scene;

namespace CodeBase.Infrastructure.Services.GameStates
{
    public class LoadMainMenuState : IEnterableState, IService
    {
        private ISceneLoader sceneLoader;
        private IWindowsProvider windowsProvider;
        private IAssetProvider assetProvider;

        public LoadMainMenuState(
            ISceneLoader sceneLoader,
            IWindowsProvider windowsProvider,
            IAssetProvider assetProvider)
        {
            this.sceneLoader = sceneLoader;
            this.windowsProvider = windowsProvider;
            this.assetProvider = assetProvider;
        }

        public void Enter()
        {
            assetProvider.CleanUp();
            
            sceneLoader.Load(Constants.MainMenuSceneName, 
                onLoaded: () => windowsProvider.Open(WindowID.MainMenuWindow));
        }
    }
}
