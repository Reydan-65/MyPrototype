using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public interface IInputInteractable : IInteractable
    {
        int InteractAmount { get; }
        float InteractionRadius { get; }
    }
}
