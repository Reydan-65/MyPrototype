using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Scene;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.GameStates
{
    public class LoadNextLevelState : IEnterableState, IService
    {
        private ISceneLoader sceneLoader;
        private IProgressProvider progressProvider;
        private IConfigsProvider configProvider;

        public LoadNextLevelState(
            ISceneLoader sceneLoader,
            IProgressProvider progressProvider,
            IConfigsProvider configProvider)
        {
            this.sceneLoader = sceneLoader;
            this.progressProvider = progressProvider;
            this.configProvider = configProvider;
        }

        public void Enter()
        {
            int levelIndex = progressProvider.PlayerProgress.CurrentLevelIndex;
            levelIndex = Mathf.Clamp(levelIndex, 0, configProvider.LevelAmount - 1);

            string sceneName = configProvider.GetLevelConfig(levelIndex).SceneName;

            sceneLoader.Load(sceneName);
        }
    }
}
