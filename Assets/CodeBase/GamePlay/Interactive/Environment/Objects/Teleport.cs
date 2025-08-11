using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Services.EntityActivityController;
using System.Collections;
using UnityEngine;

namespace CodeBase.GamePlay.Interactive
{
    [RequireComponent(typeof(Collider))]
    public class Teleport : SavableInteractable, ITriggerInteractable
    {
        [SerializeField] private Teleport connectedTeleport;
        [SerializeField] private string uniqueID = "teleport_";
        [SerializeField] private bool isActivated;

        private CharacterController characterController;
        private Collider triggerCollider;
        private bool isTeleporting;

        public override bool IsActivated { get => isActivated; set => isActivated = value; }
        public override string UniqueID => uniqueID;

        public Collider TriggerCollider => triggerCollider;

        private IEntityActivityController entityActivityController;

        [Inject]
        public void Construct(IEntityActivityController entityActivityController)
            => this.entityActivityController = entityActivityController;

        private void OnTriggerEnter(Collider other)
        {
            if (!isActivated || isTeleporting || connectedTeleport == null) return;
            if (other.gameObject != triggerCollider.gameObject) return;

            Interact();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != triggerCollider.gameObject) return;

            isTeleporting = false;
        }

        public override void Interact()
        {
            isTeleporting = true;
            connectedTeleport.isTeleporting = true;

            characterController = actionUser.GetComponent<CharacterController>();
            if (characterController != null) characterController.enabled = false;

            coroutineRunner.StartCoroutine(TeleportSequence());
        }

        private IEnumerator TeleportSequence()
        {
            entityActivityController.SetEntitiesActive(false);
            yield return new WaitForSeconds(1f);

            entityActivityController.MoveCameraToTarget(connectedTeleport.transform);
            yield return new WaitForSeconds(1f);

            actionUser.transform.position = connectedTeleport.transform.position + Vector3.up * 0.1f;
            entityActivityController.MoveCameraToTarget(actionUser.transform);
            isTeleporting = false;
            yield return new WaitForSeconds(1f);

            entityActivityController.SetEntitiesActive(true);
            if (characterController != null) characterController.enabled = true;
        }

        protected override void OnPrototypeCreated()
        {
            base.OnPrototypeCreated();

            triggerCollider = actionUser?.GetComponent<CharacterController>();
        }
    }
}
