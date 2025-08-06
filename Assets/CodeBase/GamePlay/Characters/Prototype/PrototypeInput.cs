using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeInput : MonoBehaviour
    {
        [SerializeField] private PrototypeMovement movement;
        [SerializeField] private PrototypeHealth health;
        [SerializeField] private PrototypeEnergy energy;
        [SerializeField] private PrototypeTurret turret;

        private Coroutine currentCoroutine;

        private IInputService inputService;
        private ICursorService cursorService;
        private IHealingService healingService;
        private IWindowsProvider windowsProvider;
        private IUIFactory uiFactory;
        private ICoroutineRunner coroutineRunner;

        [Inject]
        public void Construct(
            IInputService inputService,
            ICursorService cursorService,
            IHealingService healingService,
            IWindowsProvider windowsProvider,
            IUIFactory uiFactory,
            ICoroutineRunner coroutineRunner)
        {
            this.inputService = inputService;
            this.cursorService = cursorService;
            this.healingService = healingService;
            this.windowsProvider = windowsProvider;
            this.uiFactory = uiFactory;
            this.coroutineRunner = coroutineRunner;
        }

        private void Update()
        {
            if (health == null || health.IsDead) return;

            HandleMovement();
            HandleRotation();
            HandleShooting();
            HandleHealing();
            HandleDash();
            HandlePause();
        }

        private void HandlePause()
        {
            if (uiFactory == null) return;

            if (inputService.EscapeInput)
            {
                if (uiFactory.PauseWindow == null)
                    windowsProvider.Open(UI.WindowID.PauseWindow);
                else
                    uiFactory.PausePresenter.ContinueGame();
            }
        }

        private void HandleMovement()
        {
            if (movement != null)
                movement.SetMovementDirection(inputService.MovementAxis);
        }

        private void HandleRotation()
        {
            bool wasAiming = turret.IsAiming;
            bool isAimingNow = inputService.AimInput && cursorService != null;

            turret.IsAiming = isAimingNow;

            if (isAimingNow)
            {
                Vector3 cursorPos = cursorService.GetWorldPositionOnPlane(transform.position, Vector3.up);
                Vector3 lookDir = cursorPos - transform.position;
                lookDir.y = 0;

                if (lookDir != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        private void HandleShooting()
        {
            if (turret != null && turret.CanFire() && inputService.AttackInput)
            {
                turret.Fire();
                turret.IsFiring = true;

                currentCoroutine = coroutineRunner.StartCoroutine(ResetFiringFlag());
            }
        }

        private System.Collections.IEnumerator ResetFiringFlag()
        {
            yield return new WaitForSeconds(0.1f);
            turret.IsFiring = false;
        }

        private void HandleHealing()
        {
            if (inputService.HealInput && healingService != null && health.CanHeal)
                healingService.TryHeal(LootItemID.HealingPotion);
        }

        private void HandleDash()
        {
            if (inputService.DashInput && movement != null && energy != null)
                movement.TryDash();
        }
    }
}