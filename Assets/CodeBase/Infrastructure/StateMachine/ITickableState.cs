using CodeBase.Infrastructure.StateMachine;

public interface ITickableState : IState
{
    void Tick();
    //void WaitForSecond();
}