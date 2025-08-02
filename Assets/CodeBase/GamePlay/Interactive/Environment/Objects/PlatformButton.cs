using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Services.EntityActivityController;
using System.Collections;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    [RequireComponent(typeof(Collider))]
    public class PlatformButton : SavableInteractable, ITriggerInteractable
    {
        [SerializeField] private Platform connectedPlatform;
        [SerializeField] private Transform platformButtonTransform;
        [SerializeField] private Animator platformButtonAnimator;

        [SerializeField] private string uniqueID = "platform_button_";

        MeshRenderer[] meshRenderers;
        private bool isActivated;
        private Collider triggerCollider;

        public override bool IsActivated
        {
            get => isActivated;
            set
            {
                if (isActivated != value)
                {
                    isActivated = value;
                    if (isActivated) PressedButtonVisual();
                }
            }
        }

        public override string UniqueID => uniqueID;

        public Collider TriggerCollider => triggerCollider;

        private IEntityActivityController entityActivityController;

        [Inject]
        public void Construct(IEntityActivityController entityActivityController)
        {
            this.entityActivityController = entityActivityController;
        }

        protected override void Start()
        {
            base.Start();

            if (IsActivated) return;

            meshRenderers = platformButtonTransform.GetComponentsInChildren<MeshRenderer>();

            if (meshRenderers != null)
                SetButtonColor(Color.red);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerCollider == null && other != triggerCollider) return;

            Interact();
        }

        protected override void OnHeroCreated()
        {
            base.OnHeroCreated();
            triggerCollider = actionUser?.GetComponent<CharacterController>();
        }

        public override void Interact()
        {
            base.Interact();
            if (isActivated) return;
            if (connectedPlatform == null) return;

            coroutineRunner.StartCoroutine(PlatformActivationSequence());
        }

        private IEnumerator PlatformActivationSequence()
        {
            if (meshRenderers != null) PressedButtonVisual();

            entityActivityController.SetEntitiesActive(false);
            yield return new WaitForSeconds(1f);

            entityActivityController.MoveCameraToTarget(connectedPlatform.transform);
            yield return new WaitForSeconds(1f);

            connectedPlatform.Interact();
            yield return new WaitForSeconds(3f);

            entityActivityController.MoveCameraToTarget(actionUser.transform);
            yield return new WaitForSeconds(1f);

            entityActivityController.SetEntitiesActive(true);

            progressSaver.SaveProgress();
        }

        private void PressedButtonVisual()
        {
            if (meshRenderers != null)
                SetButtonColor(Color.green);
            isActivated = true;
            platformButtonAnimator.enabled = true;
        }

        private void SetButtonColor(Color color)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material.color = color;
        }

        public override void LoadProgress(PlayerProgress progress)
        {
            if (progress == null) return;

            if (progress.TryGetInteractiveState(UniqueID, out bool state))
            {
                isActivated = state;
                if (isActivated) PressedButtonVisual();
            }
        }

        public override void UpdateProgressBeforeSave(PlayerProgress progress)
        {
            progress.SetInteractiveState(UniqueID, isActivated);
        }
    }
}
