using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public interface IInteractable 
    {
        bool CanInteract();
        void Interact();
    }
}
