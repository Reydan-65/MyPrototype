using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Infrastructure.Services.GameStateMachine
{
    public interface IGameStateSwitcher : IService
    {
        object CurrentState { get; }

        void AddState<TState>(TState state) where TState : class, IState;
        void RemoveState<TState>() where TState : class, IState;
        void Enter<TState>() where TState : class, IState;
        void Exit<TState>() where TState : class, IState;
        void UpdateTick();
    }
}
