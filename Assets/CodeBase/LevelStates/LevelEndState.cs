using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.GamePlay.UI.Services;
using CodeBase.GamePlay.UI;
using CodeBase.Infrastructure.Services.ConfigProvider;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelEndState : LevelBaseState, IEnterableState
    {
        private IInputService inputService;
        private IProgressSaver progressSaver;
        private IProgressProvider progressProvider;
        private IWindowsProvider windowsProvider;

        public LevelEndState(
            IGameFactory gameFactory,
            ILevelStateSwitcher levelStateSwitcher,
            IInputService inputService,
            IProgressProvider progressProvider,
            IProgressSaver progressSaver,
            IWindowsProvider windowsProvider,
            IConfigsProvider configsProvider)
            : base(gameFactory, levelStateSwitcher)
        {
            this.inputService = inputService;
            this.progressProvider = progressProvider;
            this.progressSaver = progressSaver;
            this.windowsProvider = windowsProvider;
        }

        public void EnterAsync()
        {
            UnityEngine.Debug.Log("LEVEL: EndGame State");

            inputService.Enable = false;
            UpdateDifficultyLevel();

            progressProvider.PlayerProgress.ClearObjectsStates();
            progressSaver.ClearObjects();
            progressSaver.SaveProgress();
            windowsProvider.Open(WindowID.EndGameWindow);
        }

        private void UpdateDifficultyLevel()
        {
            progressProvider.PlayerProgress.DifficultyLevel++;
        }
    }
}
