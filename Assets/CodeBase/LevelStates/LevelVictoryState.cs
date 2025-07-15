using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.GamePlay.UI.Services;
using CodeBase.GamePlay.UI;
using CodeBase.Infrastructure.Services.ConfigProvider;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelVictoryState : LevelBaseState, IEnterableState
    {
        private IInputService inputService;
        private IProgressSaver progressSaver;
        private IProgressProvider progressProvider;
        private IWindowsProvider windowsProvider;
        private IConfigsProvider configsProvider;

        public LevelVictoryState(
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
            this.configsProvider = configsProvider;
        }

        public void Enter()
        {
            UnityEngine.Debug.Log("LEVEL: Victory State");

            inputService.Enable = false;
            //gameFactory.VirtualJoystick.gameObject.SetActive(false);

            UpdateHeroStats();
            UpdateCurrentLevelIndex();

            progressSaver.SaveProgress();

            windowsProvider.Open(WindowID.VictoryWindow);
        }

        private void UpdateCurrentLevelIndex()
        {
            progressProvider.PlayerProgress.CurrentLevelIndex++;

            //if (progressProvider.PlayerProgress.CurrentLevelIndex > configsProvider.LevelAmount - 1)
            //    progressProvider.PlayerProgress.CurrentLevelIndex = configsProvider.LevelAmount - 1;
        }

        private void UpdateHeroStats()
        {
            progressProvider.PlayerProgress.HeroStats.Damage += 2;
            progressProvider.PlayerProgress.HeroStats.MaxHitPoints += 5;
            progressProvider.PlayerProgress.HeroStats.MovementSpeed += 0.1f;
        }
    }
}
