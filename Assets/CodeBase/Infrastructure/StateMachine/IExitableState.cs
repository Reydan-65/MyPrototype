using CodeBase.Infrastructure.StateMachine;

public interface IExitableState : IState
{
    void Exit();
}