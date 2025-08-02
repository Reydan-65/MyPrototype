using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public interface ITriggerInteractable : IInteractable
    {
        Collider TriggerCollider { get; }
    }
}
