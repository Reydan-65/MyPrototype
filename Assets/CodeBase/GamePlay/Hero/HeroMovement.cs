using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;
using UnityEngine.EventSystems;

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

        private DashController dashController;
        private GravityHandler gravityHandler;
        private HeroHealth heroHealth;
        private HeroEnergy heroEnergy;

        private float movementSpeed;
        private Vector3 directionControl;
        public Vector3 DirectionControl => directionControl;

        [Header("Dash Settings")]
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private AnimationCurve dashSpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private int dashCost = 30;

        private float dashRange;

        private IInputService inputService;
        private ICursorService cursorService;

        private void Awake()
        {
            heroHealth = GetComponent<HeroHealth>();
            heroEnergy = GetComponent<HeroEnergy>();

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

        public void Initialize(float movementSpeed, float dashRange)
        {
            this.movementSpeed = movementSpeed;
            this.dashRange = dashRange;

            dashController = new DashController(
                characterController,
                heroHealth,
                this.dashRange,
                dashDuration,
                dashCooldown,
                dashSpeedCurve);
        }

        private void Update()
        {
            if (dashController == null) return;

            float deltaTime = Time.deltaTime;

            gravityHandler.UpdateGravity();
            dashController.UpdateCooldown(deltaTime);

            if (dashController.IsDashing)
                dashController.UpdateDash(deltaTime, gravityHandler.VerticalVelocity);
            else
                MoveCharacter();

            RotateView();
        }

        private void MoveCharacter()
        {
            Vector3 moveDirection = directionControl * movementSpeed;
            moveDirection.y = gravityHandler.VerticalVelocity.y;

            if (inputService != null && heroHealth != null)
            {
                if (CanDash())
                {
                    heroEnergy.Consume(dashCost);
                    dashController.StartDash(moveDirection);
                }
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }

        public bool TryDash()
        {
            if (!CanDash()) return false;

            heroEnergy.Consume(dashCost);
            dashController.StartDash(inputService.MovementAxis);

            return true;
        }

        private bool CanDash()
        {
            return dashController.CanDash &&
                   heroEnergy.Current >= dashCost &&
                   inputService != null &&
                   inputService.DashInput &&
                   directionControl.magnitude > 0.1f;
        }

        private void RotateView()
        {
            if (inputService == null || cursorService == null) return;

            if (inputService.AimInput)
            {
                Vector3 cursorWorldPos = cursorService.GetWorldPositionOnPlane(viewTransform.position, Vector3.up);
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
            Initialize(progress.HeroStats.MovementSpeed, progress.HeroStats.DashRange);
        }
    }
}