using CodeBase.Infrastructure.Services.LevelStates;
using CodeBase.Infrastructure.DependencyInjection;

namespace CodeBase.Infrastructure
{
    public class LevelBootStrapper : MonoBootStrapper
    {
        private ILevelStateSwitcher levelStateSwitcher;
        private LevelBootStrapState levelBootStrappState;
        private LevelResearchState levelResearchState;
        private LevelVictoryState levelVictoryState;
        private LevelDeathState levelDeathState;
        private LevelBattleState levelCombatState;
        private LevelRespawnState levelRespawnState;

        [Inject]
        public void Construct(
            ILevelStateSwitcher levelStateSwitcher,
            LevelBootStrapState levelBootStrappState,
            LevelResearchState levelResearchState,
            LevelVictoryState levelVictoryState,
            LevelDeathState levelDeathState,
            LevelBattleState levelCombatState,
            LevelRespawnState levelRespawnState)
        {
            this.levelStateSwitcher = levelStateSwitcher;
            this.levelBootStrappState = levelBootStrappState;
            this.levelResearchState = levelResearchState;
            this.levelVictoryState = levelVictoryState;
            this.levelDeathState = levelDeathState;
            this.levelCombatState = levelCombatState;
            this.levelRespawnState = levelRespawnState;
        }

        public override void OnBindResolved() => InitLevelStateMachine();

        private void InitLevelStateMachine()
        {
            levelStateSwitcher.AddState(levelBootStrappState);
            levelStateSwitcher.AddState(levelResearchState);
            levelStateSwitcher.AddState(levelVictoryState);
            levelStateSwitcher.AddState(levelDeathState);
            levelStateSwitcher.AddState(levelCombatState);
            levelStateSwitcher.AddState(levelRespawnState);

            levelStateSwitcher.Enter<LevelBootStrapState>();
        }
    }
}
