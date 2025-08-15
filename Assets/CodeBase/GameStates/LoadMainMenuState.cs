using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Scene;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Sounds;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.GameStates
{
    public class LoadMainMenuState : IEnterableState, IService
    {
        private ISceneLoader sceneLoader;
        private IWindowsProvider windowsProvider;
        private IAssetProvider assetProvider;
        private IGameFactory gameFactory;

        public LoadMainMenuState(
            ISceneLoader sceneLoader,
            IWindowsProvider windowsProvider,
            IAssetProvider assetProvider,
            IGameFactory gameFactory)
        {
            this.sceneLoader = sceneLoader;
            this.windowsProvider = windowsProvider;
            this.assetProvider = assetProvider;
            this.gameFactory = gameFactory;
        }

        public void EnterAsync()
        {
            assetProvider.CleanUp();

            sceneLoader.Load(Constants.MainMenuSceneName,
                onLoaded: async () =>
                {
                    windowsProvider.Open(WindowID.MainMenuWindow);
                    await CreateAudioPlayer();
                });
        }

        private async Task CreateAudioPlayer()
        {
            AudioPlayer audioPlayer = await gameFactory.CreateAudioPlayerAsync();
            if (audioPlayer != null)
            {
                MusicPlayer musicPlayer = audioPlayer.GetComponent<MusicPlayer>();
                SFXPlayer sfxPlayer = audioPlayer.GetComponent<SFXPlayer>();

                musicPlayer?.UpdateAudioVolume();
                sfxPlayer?.UpdateAudioVolume();

                musicPlayer.PlayMusic(await assetProvider.Load<AudioClip>(AssetAddress.MainMenuMusic));
            }
        }
    }
}