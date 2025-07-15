using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMovement : MonoBehaviour, IProgressLoadHandler
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private float gravity = 30f;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private float movementSpeed;
        private float dashRange;
        private HeroHealth heroHealth;
        private Vector3 directionControl;
        private GravityHandler gravityHandler;
        public Vector3 DirectionControl => directionControl;

        private IInputService inputService;
        private ICursorService cursorService;

        private void Awake()
        {
            heroHealth = GetComponent<HeroHealth>();
            gravityHandler = new GravityHandler(
                characterController,
                gravity,
                groundCheckDistance,
                groundLayer
                );
        }

        [Inject]
        public void Construct(
            IInputService inputService,
            ICursorService cursorService)
        {
            this.inputService = inputService;
            this.cursorService = cursorService;
        }

        private void Update()
        {
            gravityHandler.UpdateGravity();

            MoveCharacter();
            RotateView();
        }

        private void MoveCharacter()
        {
            Vector3 moveDirection = directionControl * movementSpeed;
            moveDirection.y = gravityHandler.VerticalVelocity.y;

            if (inputService != null && heroHealth != null)
            {
                if (inputService.DashInput && heroHealth.Current > 0)
                {
                    heroHealth.SetInvulnerability(true);
                    characterController.Move(moveDirection * Time.deltaTime * dashRange);
                }
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }

        private void RotateView()
        {
            if (inputService == null || cursorService == null) return;

            if (inputService.AimInput)
            {
                Vector3 cursorWorldPos = cursorService.GetWorldPositionOnPlane(viewTransform.position,Vector3.up);
                Vector3 lookDirection = cursorWorldPos - viewTransform.position;
                lookDirection.y = 0;

                if (lookDirection != Vector3.zero)
                    viewTransform.rotation = Quaternion.LookRotation(lookDirection);

                return;
            }

            if (directionControl.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionControl);
                viewTransform.rotation = targetRotation;
            }
        }

        public void SetMovementDirection(Vector2 moveDirection)
        {
            directionControl.x = moveDirection.x;
            directionControl.z = moveDirection.y;
            directionControl.y = 0;

            directionControl.Normalize();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            movementSpeed = progress.HeroStats.MovementSpeed;
            dashRange = progress.HeroStats.DashRange;
        }
    }
}