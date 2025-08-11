using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    [RequireComponent(typeof(CharacterController))]
    public class PrototypeMovement : MonoBehaviour, IProgressLoadHandler
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private float gravity = 30f;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private DashController dashController;
        private GravityHandler gravityHandler;
        private PrototypeHealth prototypeHealth;
        private PrototypeEnergy prototypeEnergy;

        private float movementSpeed;
        private Vector3 directionControl;
        public Vector3 DirectionControl => directionControl;

        [Header("Dash Settings")]
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private AnimationCurve dashSpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private int dashCost = 30;

        private float dashRange;

        private VisualModelTilt visualTilt;
        private FirePointStabilizer firePointStabilizer;

        private IInputService inputService;
        private ICursorService cursorService;

        private void Awake()
        {
            prototypeHealth = GetComponent<PrototypeHealth>();
            prototypeEnergy = GetComponent<PrototypeEnergy>();
            visualTilt = GetComponent<VisualModelTilt>();
            firePointStabilizer = GetComponent<FirePointStabilizer>();

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

            IMovementController movementController = new CharacterMovementController(characterController);
            dashController = new DashController(
                movementController,
                prototypeHealth,
                dashRange,
                dashDuration,
                dashCooldown,
                dashSpeedCurve
            );
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

            visualTilt.UpdateTilt(directionControl);
            firePointStabilizer.ResetLocalRotation();
            firePointStabilizer.Stabilize();
        }

        private void MoveCharacter()
        {
            Vector3 moveDirection = directionControl * movementSpeed;
            moveDirection.y = gravityHandler.VerticalVelocity.y;

            if (characterController != null && characterController.enabled)
                characterController.Move(moveDirection * Time.deltaTime);
        }

        public bool TryDash()
        {
            if (!CanDash()) return false;

            prototypeEnergy.Consume(dashCost);
            dashController.StartDash(directionControl);

            return true;
        }

        private bool CanDash()
        {
            return dashController.CanDash &&
                   prototypeEnergy.Current >= dashCost &&
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
            Initialize(progress.PrototypeStats.MovementSpeed.Value, progress.PrototypeStats.DashRange.Value);
        }
    }
}