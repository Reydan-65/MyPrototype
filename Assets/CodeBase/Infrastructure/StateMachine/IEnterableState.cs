using CodeBase.Infrastructure.StateMachine;

public interface IEnterableState : IState
{
    void Enter();
}