using UnityEngine;
using UnityEngine.Events;

namespace Assets.CodeBase.GamePlay.Interactive
{
    public interface IInteractable
    {
        event UnityAction InteractionStarted;
        event UnityAction InteractionEnded;

        Transform InteractionPoint {  get; }
        float InteractionRadius { get; }
        bool IsInteractable { get; }

        bool CanInteract(GameObject user);
        void Interact(GameObject user);
    }
}
