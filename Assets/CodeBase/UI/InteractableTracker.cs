using CodeBase.GamePlay.Interactive;
using CodeBase.GamePlay.Prototype;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class InteractableTracker : MonoBehaviour
    {
        [SerializeField] private HUDWindow hudWindow;
        [SerializeField] private List<IInputInteractable> inputInteractables = new List<IInputInteractable>();
        [SerializeField] private float checkInterval = 0.1f;

        private bool previousState;
        private bool isInitialized;

        private PrototypeInput input;
        private Coroutine checkCoroutine;

        private ICoroutineRunner coroutineRunner;

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner) 
        { 
            this.coroutineRunner = coroutineRunner;
            isInitialized = true;

            if (isActiveAndEnabled)
                StartChecking();
        }

        private void OnEnable()
        {
            if (isInitialized)
                StartChecking();
        }

        private void OnDisable() => StopChecking();

        public void StartChecking()
        {
            StopChecking();

            if (coroutineRunner != null)
                checkCoroutine = coroutineRunner.StartCoroutine(CheckInteractablesRoutine());
        }

        public void StopChecking()
        {
            if (checkCoroutine != null)
            {
                StopAllCoroutines();
                checkCoroutine = null;
            }
        }

        private IEnumerator CheckInteractablesRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(checkInterval);
                CheckInteractables();
            }
        }

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

                    if (input.HasOpenedWindow) return;

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

        public void SetInput(PrototypeInput input) => this.input = input;
    }
}