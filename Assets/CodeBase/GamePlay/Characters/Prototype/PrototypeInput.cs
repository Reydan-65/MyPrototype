using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Sounds;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeInput : MonoBehaviour
    {
        public SFXEvent PlaySFX;

        [SerializeField] private PrototypeMovement movement;
        [SerializeField] private PrototypeHealth health;
        [SerializeField] private PrototypeEnergy energy;
        [SerializeField] private PrototypeTurret turret;

        private bool isPaused = false;
        public bool IsPaused { get => isPaused; set => isPaused = value; }

        private IInputService inputService;
        private ICursorService cursorService;
        private IHealingService healingService;
        private IWindowsProvider windowsProvider;
        private ICoroutineRunner coroutineRunner;
        private IAssetProvider assetProvider;

        [Inject]
        public void Construct(
            IInputService inputService,
            ICursorService cursorService,
            IHealingService healingService,
            IWindowsProvider windowsProvider,
            IUIFactory uiFactory,
            ICoroutineRunner coroutineRunner,
            IAssetProvider assetProvider)
        {
            this.inputService = inputService;
            this.cursorService = cursorService;
            this.healingService = healingService;
            this.windowsProvider = windowsProvider;
            this.coroutineRunner = coroutineRunner;
            this.assetProvider = assetProvider;
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
            if (inputService.EscapeInput)
            {
                if (!isPaused)
                {
                    isPaused = true;
                    windowsProvider.Open(UI.WindowID.PauseWindow);
                }
                else
                    return;
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

                coroutineRunner.StartCoroutine(ResetFiringFlag());
            }
        }

        private System.Collections.IEnumerator ResetFiringFlag()
        {
            yield return new WaitForSeconds(0.1f);
            turret.IsFiring = false;
        }

        private async void HandleHealing()
        {
            if (inputService.HealInput && healingService != null && health.CanHeal)
            {
                bool healed = healingService.TryHeal(LootItemID.HealingPotion);

                if (healed)
                    PlaySFX?.Invoke(await assetProvider.Load<AudioClip>(AssetAddress.UsePotionSound), 0.5f, 1, 1);
            }
        }

        private async void HandleDash()
        {
            if (inputService.DashInput && movement != null && energy != null)
                await movement.TryDash();
        }
    }
}