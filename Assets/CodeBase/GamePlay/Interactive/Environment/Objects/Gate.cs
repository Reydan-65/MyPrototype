using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System.Collections;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    public class Gate : SavableInteractable, IInputInteractable
    {
        [SerializeField] private Animator gateAnimator;
        [SerializeField] private string requiredKeyID;
        [SerializeField] private GameObject keyRequiredMessage;
        [SerializeField] private string uniqueID = "gate_";

        private Coroutine messageCoroutine;
        private Coroutine loadStateCoroutine;
        private bool isActivated;

        public override bool IsActivated
        {
            get => isActivated;
            set
            {
                if (isActivated != value)
                {
                    isActivated = value;
                    if (isActivated) OpenGateVisual();
                }
            }
        }

        public override string UniqueID => uniqueID;

        private IProgressProvider progressProvider;

        [Inject]
        public void Counstruct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;
        }

        protected override void Start()
        {
            base.Start();
            keyRequiredMessage?.SetActive(false);
        }

        public override void Interact()
        {
            base.Interact();
            if (IsActivated) return;

            if (!HasRequiredKey())
            {
                ShowKeyRequiredMessage();
                return;
            }

            IsActivated = true;
            Process();
            progressSaver.SaveProgress();
        }

        private bool HasRequiredKey()
        {
            return progressProvider?.PlayerProgress?.PrototypeInventoryData?.Keys.Contains(requiredKeyID) ?? false;
        }

        private void ShowKeyRequiredMessage()
        {
            if (keyRequiredMessage == null) return;

            keyRequiredMessage.SetActive(true);
            if (messageCoroutine != null)
                StopCoroutine(messageCoroutine);

            messageCoroutine = StartCoroutine(HideMessageAfterCooldown());
        }

        private IEnumerator HideMessageAfterCooldown()
        {
            yield return new WaitForSeconds(interactCooldown);
            keyRequiredMessage.SetActive(false);
            messageCoroutine = null;
        }

        private void Process()
        {
            progressProvider.PlayerProgress.PrototypeInventoryData.RemoveKey(requiredKeyID);
            interactAmount--;
            OpenGateVisual();
        }

        private void OpenGateVisual()
        {
            gateAnimator.enabled = true;
            DisableKeyChecker();
        }

        private void DisableKeyChecker()
        {
            KeyChecker checker = GetComponentInChildren<KeyChecker>();
            if (checker != null)
            {
                checker.UnsubscribeFromEvents();
                checker.gameObject.SetActive(false);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (interactableTracker != null)
                interactableTracker.UnregisterInteractable(this);
        }

        protected override void OnPrototypeCreated()
        {
            base.OnPrototypeCreated();
            loadStateCoroutine = coroutineRunner.StartCoroutine(DelayedLoadCheck());
        }

        private IEnumerator DelayedLoadCheck()
        {
            yield return null;

            if (progressProvider?.PlayerProgress != null)
            {
                if (progressProvider.PlayerProgress.TryGetInteractiveState(UniqueID, out bool state))
                {
                    IsActivated = state;
                    if (isActivated) OpenGateVisual();
                }
            }

            loadStateCoroutine = null;
        }

        protected override void OnHUDWindowCreated()
        {
            base.OnHUDWindowCreated();

            if (interactableTracker != null)
                interactableTracker.RegisterInteractable(this);
        }
        public override void LoadProgress(PlayerProgress progress)
        {
            if (progress == null) return;

            if (progress.TryGetInteractiveState(UniqueID, out bool state))
            {
                IsActivated = state;
                if (isActivated) OpenGateVisual();
            }
        }

        public override void UpdateProgressBeforeSave(PlayerProgress progress)
        {
            progress.SetInteractiveState(UniqueID, IsActivated);
        }
    }
}