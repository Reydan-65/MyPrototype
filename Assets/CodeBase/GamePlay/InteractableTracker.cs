using CodeBase.GamePlay.Interactive;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class InteractableTracker : MonoBehaviour
    {
        [SerializeField] private HUDWindow hudWindow;
        [SerializeField] private List<IInputInteractable> inputInteractables = new List<IInputInteractable>();

        private bool previousState;

        private void Update() => CheckInteractables();

        public void RegisterInteractable(IInputInteractable interactable)
        {
            if (!inputInteractables.Contains(interactable))
                inputInteractables.Add(interactable);
        }

        public void UnregisterInteractable(IInputInteractable interactable)
        {
            if (inputInteractables.Contains(interactable))
                inputInteractables.Remove(interactable);
        }

        private void CheckInteractables()
        {
            if (hudWindow == null || hudWindow.Solution == null)
                return;

            bool hasActiveInteractable = false;

            foreach (var interactable in inputInteractables)
            {
                if (interactable != null &&
                    interactable.InteractAmount != 0 &&
                    interactable.CanInteract())
                {
                    if (interactable is Gate gate)
                        if (gate.IsActivated) return;
                    hasActiveInteractable = true;
                    break;
                }
            }

            if (hasActiveInteractable != previousState)
            {
                hudWindow.SetSolutionActive(hasActiveInteractable);
                previousState = hasActiveInteractable;
            }
        }
    }
}