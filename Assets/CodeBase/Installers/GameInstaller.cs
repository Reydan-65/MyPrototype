using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Scene;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.ConfigProvider;
using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.GameStateMachine;
using CodeBase.Infrastructure.Services.GameStates;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using CodeBase.Infrastructure.Services.SettingsProvider;
using CodeBase.Infrastructure.Services.SettingsSaver;
using CodeBase.Services.EntityActivityController;

namespace CodeBase.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //UnityEngine.Debug.Log("GLOBAL: Install");

            RegisterGameServices();
            RegisterGameStateMachine();
        }

        private void RegisterGameStateMachine()
        {
            container.RegisterSingle<IGameStateSwitcher, GameStateMachine>();
            container.RegisterSingle<GameBootStrapState>();
            container.RegisterSingle<LoadLevelState>();
            container.RegisterSingle<LoadMainMenuState>();
        }

        private void RegisterGameServices()
        {
            container.RegisterSingle<IConfigsProvider, ConfigsProvider>();
            container.RegisterSingle<ICoroutineRunner, CoroutineRunner>();
            container.RegisterSingle<IAssetProvider, AssetProvider>();
            container.RegisterSingle<IProgressProvider, ProgressProvider>();
            container.RegisterSingle<IProgressSaver, ProgressSaver>();
            container.RegisterSingle<ISettingsProvider, SettingsProvider>();
            container.RegisterSingle<ISettingsSaver, SettingsSaver>();
            container.RegisterSingle<ISceneLoader, SceneLoader>();
            container.RegisterSingle<IInputService, InputService>();
            container.RegisterSingle<ICursorService, CursorService>();
            container.RegisterSingle<IEnemySpawnManager, EnemySpawnManager>();
            container.RegisterSingle<IGameFactory, GameFactory>();
            container.RegisterSingle<IUIFactory, UIFactory>();
            container.RegisterSingle<ILootService, LootService>();
            container.RegisterSingle<IWindowsProvider, WindowsProvider>();
            //container.RegisterSingle<IAdsService, AdsService>();
            container.RegisterSingle<IIApService, IApService>();
            container.RegisterSingle<IHealingService, HealingService>();
            container.RegisterSingle<IEntityActivityController, EntityActivityController>();
        }
    }
}
