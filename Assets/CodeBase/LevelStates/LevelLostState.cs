using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.Services.Factory;
using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.LevelStates
{
    public class LevelLostState : LevelBaseState, IEnterableState
    {
        private const float DELAY = 3f;

        private IInputService inputService;
        private IWindowsProvider windowsProvider;
        private ICoroutineRunner coroutineRunner;
        private IUIFactory uiFactory;

        public LevelLostState(
            IGameFactory gameFactory,
            ILevelStateSwitcher levelStateSwitcher,
            IInputService inputService,
            IWindowsProvider windowsProvider,
            ICoroutineRunner coroutineRunner,
            IUIFactory uiFactory)
            : base(gameFactory, levelStateSwitcher)
        {
            this.inputService = inputService;
            this.windowsProvider = windowsProvider;
            this.coroutineRunner = coroutineRunner;
            this.uiFactory = uiFactory;
        }

        public void EnterAsync()
        {
            Debug.Log("LEVEL: Lost State");

            inputService.Enable = false;
            uiFactory.HUDWindow.Close();
            //gameFactory.VirtualJoystick.gameObject.SetActive(false);

            coroutineRunner.StartCoroutine(SwitchStateDelay());
        }

        private IEnumerator SwitchStateDelay()
        {
            yield return new WaitForSeconds(DELAY);

            levelStateSwitcher.Enter<LevelRespawnState>();
        }
    }
}
