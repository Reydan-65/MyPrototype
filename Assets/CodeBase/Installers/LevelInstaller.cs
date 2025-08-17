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
            container.RegisterSingle<LevelEndState>();
            container.RegisterSingle<LevelDeathState>();
            container.RegisterSingle<LevelBattleState>();
            container.RegisterSingle<LevelRespawnState>();
        }

        private void UnregisterLevelStateMachine()
        {
            container.UnregisterSingle<ILevelStateSwitcher>();

            container.UnregisterSingle<LevelBootStrapState>();
            container.UnregisterSingle<LevelResearchState>();
            container.UnregisterSingle<LevelEndState>();
            container.UnregisterSingle<LevelDeathState>();
            container.UnregisterSingle<LevelBattleState>();
            container.UnregisterSingle<LevelRespawnState>();
        }
    }
}
