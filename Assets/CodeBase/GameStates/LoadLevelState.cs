using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.GameStates
{
    public class LoadLevelState : IEnterableState, IService
    {
        private ISceneTransition sceneTransition;
        private IProgressProvider progressProvider;
        private IConfigsProvider configProvider;

        public LoadLevelState(
            ISceneTransition sceneTransition,
            IProgressProvider progressProvider,
            IConfigsProvider configProvider)
        {
            this.sceneTransition = sceneTransition;
            this.progressProvider = progressProvider;
            this.configProvider = configProvider;
        }

        public void EnterAsync()
        {
            int levelIndex = progressProvider.PlayerProgress.DifficultyLevel;
            levelIndex = Mathf.Clamp(levelIndex, 0, configProvider.LevelAmount - 1);

            string sceneName = configProvider.GetLevelConfig(levelIndex).SceneName;
            sceneTransition.LoadSceneWithTransition(sceneName);
        }
    }
}
