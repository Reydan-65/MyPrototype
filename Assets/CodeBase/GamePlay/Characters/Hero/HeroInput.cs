using CodeBase.Data;
using CodeBase.GamePlay.UI.Services;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using System;
using UnityEngine;

namespace CodeBase.GamePlay.Hero
{
    public class HeroInput : MonoBehaviour
    {
        [SerializeField] private HeroMovement movement;
        [SerializeField] private HeroHealth health;
        [SerializeField] private HeroEnergy energy;
        [SerializeField] private HeroTurret turret;

        private IInputService inputService;
        private ICursorService cursorService;
        private IHealingService healingService;
        private IWindowsProvider windowsProvider;
        private IUIFactory uiFactory;

        [Inject]
        public void Construct(
            IInputService inputService,
            ICursorService cursorService,
            IHealingService healingService,
            IWindowsProvider windowsProvider,
            IUIFactory uiFactory)
        {
            this.inputService = inputService;
            this.cursorService = cursorService;
            this.healingService = healingService;
            this.windowsProvider = windowsProvider;
            this.uiFactory = uiFactory;
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
            if (inputService.AimInput && cursorService != null)
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
                turret.Fire();
        }

        private void HandleHealing()
        {
            if (inputService.HealInput && healingService != null)
                healingService.TryHeal(LootItemID.HealingPotion);
        }

        private void HandleDash()
        {
            if (inputService.DashInput && movement != null && energy != null)
                movement.TryDash();
        }
    }
}