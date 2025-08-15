using CodeBase.GamePlay.UI;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Factory;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class Interactable : MonoBehaviour
    {
        public event UnityAction InteractionStarted;
        public event UnityAction InteractionEnded;

        [SerializeField] protected Transform interactPoint;
        [SerializeField] protected float interactRadius = 2f;
        [SerializeField] protected float interactCooldown = 3f;
        [SerializeField] protected int interactAmount = -1;

        protected GameObject actionUser;
        protected Coroutine currentCoroutine;
        private IGameFactory gameFactory;
        protected IInputService inputService;
        protected ICoroutineRunner coroutineRunner;
        private IUIFactory uiFactory;
        private HUDWindow hudWindow;
        protected InteractableTracker interactableTracker;

        public Transform InteractionPoint => interactPoint;
        public float InteractionRadius => interactRadius;
        public bool IsInteractable { get; protected set; } = true;
        public int InteractAmount => interactAmount;

        [Inject]
        public virtual void Construct(
            IGameFactory gameFactory,
            IInputService inputService,
            ICoroutineRunner coroutineRunner,
            IUIFactory uiFactory)
        {
            this.gameFactory = gameFactory;
            this.inputService = inputService;
            this.coroutineRunner = coroutineRunner;
            this.uiFactory = uiFactory;
        }

        protected virtual void Start()
        {
            if (interactPoint == null)
                interactPoint = transform;
            if (gameFactory != null)
                gameFactory.PrototypeCreated += OnPrototypeCreated;

            uiFactory.HUDWindowCreated += OnHUDWindowCreated;
        }

        protected virtual void OnHUDWindowCreated()
        {
            hudWindow = uiFactory.HUDWindow;
            interactableTracker = hudWindow.GetComponent<InteractableTracker>();
            uiFactory.HUDWindowCreated -= OnHUDWindowCreated;
        }

        protected virtual void OnDestroy()
        {
            StopAllCoroutines();

            if (gameFactory != null)
                gameFactory.PrototypeCreated -= OnPrototypeCreated;
        }

        protected virtual void Update()
        {
            if (interactAmount == 0) return;
            if (actionUser == null || !IsInteractable) return;

            if (!inputService.UseInput) return;
            if (CanInteract()) Interact();
        }

        protected virtual void OnPrototypeCreated() => actionUser = gameFactory.PrototypeObject;

        public virtual bool CanInteract()
        {
            if (actionUser == null || !IsInteractable) return false;
            float distance = Vector3.Distance(interactPoint.position, actionUser.transform.position);
            return distance <= interactRadius;
        }

        public virtual void Interact() => StartCoroutine();

        protected virtual IEnumerator InteractionProcess()
        {
            yield return new WaitForSeconds(interactCooldown);
            CompleteInteraction();
            currentCoroutine = null;
        }

        protected virtual void StartCoroutine()
        {
            IsInteractable = false;
            InteractionStarted?.Invoke();
            currentCoroutine = coroutineRunner.StartCoroutine(InteractionProcess());
        }

        protected virtual void CompleteInteraction()
        {
            IsInteractable = true;
            currentCoroutine = null;
            InteractionEnded?.Invoke();
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (interactPoint == null) return;

            Gizmos.color = IsInteractable ? Color.green : Color.red;
            Gizmos.DrawWireSphere(interactPoint.position, interactRadius);
        }
#endif
    }
}
