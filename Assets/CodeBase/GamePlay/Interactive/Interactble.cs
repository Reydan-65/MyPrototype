using Assets.CodeBase.GamePlay.Interactive;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Factory;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.GamePlay.Interactive
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public event UnityAction InteractionStarted;
        public event UnityAction InteractionEnded;

        [SerializeField] protected Transform interactPoint;
        [SerializeField] protected float interactRadius = 2f;
        [SerializeField] protected float interactCooldown = 3f;

        protected GameObject actionUser;
        protected Coroutine currentCoroutine;
        private IGameFactory gameFactory;
        protected IInputService inputService;
        protected ICoroutineRunner coroutineRunner;

        public Transform InteractionPoint => interactPoint;
        public float InteractionRadius => interactRadius;
        public bool IsInteractable { get; protected set; } = true;

        [Inject]
        public virtual void Construct(
            IGameFactory gameFactory,
            IInputService inputService,
            ICoroutineRunner coroutineRunner)
        {
            this.gameFactory = gameFactory;
            this.inputService = inputService;
            this.coroutineRunner = coroutineRunner;
        }

        protected virtual void Start()
        {
            if (interactPoint == null)
                interactPoint = transform;
            if (gameFactory != null)
                gameFactory.HeroCreated += OnHeroCreated;
        }

        protected virtual void OnDestroy()
        {
            if (currentCoroutine != null)
                coroutineRunner.StopCoroutine(currentCoroutine);
            if (gameFactory != null)
                gameFactory.HeroCreated -= OnHeroCreated;
        }

        protected virtual void Update()
        {
            if (actionUser == null || !IsInteractable || !inputService.UseInput) return;
            if (CanInteract(actionUser)) Interact(actionUser);
        }

        private void OnHeroCreated() => actionUser = gameFactory.HeroObject;

        public virtual bool CanInteract(GameObject user)
        {
            if (user == null || !IsInteractable) return false;
            float distance = Vector3.Distance(interactPoint.position, user.transform.position);
            return distance <= interactRadius;
        }

        public virtual void Interact(GameObject user)
        {
            if (!CanInteract(user)) return;
            StartCoroutine();
        }


        protected virtual IEnumerator InteractionProcess()
        {
            yield return new WaitForSeconds(interactCooldown);
            CompleteInteraction();
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