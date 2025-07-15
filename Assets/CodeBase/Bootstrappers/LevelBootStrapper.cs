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
        private LevelLostState levelLostState;
        private LevelBattleState levelCombatState;

        [Inject]
        public void Construct(
            ILevelStateSwitcher levelStateSwitcher,
            LevelBootStrapState levelBootStrappState,
            LevelResearchState levelResearchState,
            LevelVictoryState levelVictoryState,
            LevelLostState levelLostState,
            LevelBattleState levelCombatState)
        {
            this.levelStateSwitcher = levelStateSwitcher;
            this.levelBootStrappState = levelBootStrappState;
            this.levelResearchState = levelResearchState;
            this.levelVictoryState = levelVictoryState;
            this.levelLostState = levelLostState;
            this.levelCombatState = levelCombatState;
        }

        public override void OnBindResolved() => InitLevelStateMachine();

        private void InitLevelStateMachine()
        {
            levelStateSwitcher.AddState(levelBootStrappState);
            levelStateSwitcher.AddState(levelResearchState);
            levelStateSwitcher.AddState(levelVictoryState);
            levelStateSwitcher.AddState(levelLostState);
            levelStateSwitcher.AddState(levelCombatState);

            levelStateSwitcher.Enter<LevelBootStrapState>();
        }
    }
}
