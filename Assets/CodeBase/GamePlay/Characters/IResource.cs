using UnityEngine.Events;

namespace CodeBase.GamePlay
{
    public interface IResource
    {
        event UnityAction Changed;
        event UnityAction Depleted;

        float Max { get; }
        float Current { get; }
    }
}