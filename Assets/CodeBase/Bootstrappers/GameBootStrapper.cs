using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.GameStateMachine;

namespace CodeBase.Infrastructure
{
    public class GameBootStrapper : MonoBootStrapper
    {
        private IGameStateSwitcher gameStateSwitcher;
        private GameBootStrapState gameBootStrappState;
        private LoadLevelState loadNextLevelState;
        private LoadMainMenuState loadMainMenuState;

        [Inject]
        public void Construct(
            IGameStateSwitcher gameStateSwitcher, 
            GameBootStrapState gameBootStrappState, 
            LoadLevelState loadLevelState,
            LoadMainMenuState loadMainMenuState)
        {
            this.gameStateSwitcher = gameStateSwitcher;
            this.gameBootStrappState = gameBootStrappState;
            this.loadNextLevelState = loadLevelState;
            this.loadMainMenuState = loadMainMenuState;
        }

        public override void OnBindResolved() => InitGameStateMachine();

        private void InitGameStateMachine()
        {
            gameStateSwitcher.AddState(gameBootStrappState);
            gameStateSwitcher.AddState(loadNextLevelState);
            gameStateSwitcher.AddState(loadMainMenuState);

            gameStateSwitcher.Enter<GameBootStrapState>();
        }
    }
}
