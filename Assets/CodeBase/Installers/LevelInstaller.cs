using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.LevelStates;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelStateMachineTicker levelStateMachineTicker;

        public override void InstallBindings()
        {
            //Debug.Log("LEVEL: Install");

            container.RegisterSingle(levelStateMachineTicker);

            RegisterLevelStateMachine();
        }

        private void OnDestroy()
        {
            if (container == null) return;

            container.UnregisterSingle<LevelStateMachineTicker>();
            
            UnregisterLevelStateMachine();
        }

        private void RegisterLevelStateMachine()
        {
            container.RegisterSingle<ILevelStateSwitcher, LevelStateMachine>();
            container.RegisterSingle<LevelBootStrapState>();
            container.RegisterSingle<LevelResearchState>();
            container.RegisterSingle<LevelVictoryState>();
            container.RegisterSingle<LevelLostState>();
            container.RegisterSingle<LevelBattleState>();
        }

        private void UnregisterLevelStateMachine()
        {
            container.UnregisterSingle<ILevelStateSwitcher>();
            container.UnregisterSingle<LevelBootStrapState>();
            container.UnregisterSingle<LevelResearchState>();
            container.UnregisterSingle<LevelVictoryState>();
            container.UnregisterSingle<LevelLostState>();
            container.UnregisterSingle<LevelBattleState>();
        }
    }
}
