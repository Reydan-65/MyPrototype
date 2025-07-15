using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.LevelStates;
using UnityEngine;

public class LevelStateMachineTicker : MonoBehaviour, IService
{
    private ILevelStateSwitcher levelStateSwitcher;

    [Inject]
    public void Construct(ILevelStateSwitcher levelStateSwitcher)
    {
        this.levelStateSwitcher = levelStateSwitcher;
    }

    public void Update()
    {
        levelStateSwitcher.UpdateTick();
    }
}
