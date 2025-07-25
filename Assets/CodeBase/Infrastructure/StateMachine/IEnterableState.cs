using CodeBase.Infrastructure.StateMachine;

public interface IEnterableState : IState
{
    void EnterAsync();
}