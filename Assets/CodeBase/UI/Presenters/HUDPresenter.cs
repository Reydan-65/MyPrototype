using CodeBase.GamePlay.Prototype;
using CodeBase.Infrastructure.Services.Factory;
using UnityEngine;

namespace CodeBase.GamePlay.UI
{
    public class HUDPresenter : WindowPresenterBase<HUDWindow>
    {
        private HUDWindow window;
        private PrototypeHealth prototypeHealth;

        private IGameFactory gameFactory;

        public HUDPresenter(IGameFactory gameFactory)
            => this.gameFactory = gameFactory;

        public override void SetWindow(HUDWindow window)
        {
            this.window = window;
            this.window.LockImage.fillAmount = 0;
            this.window.ControlsAnimator.enabled = true;

            UpdateHUDDisplay();
        }

        private void UpdateHUDDisplay()
        {
            if (window == null || gameFactory.PrototypeObject == null)
                return;

            window.GetComponent<InteractableTracker>().SetInput(gameFactory.PrototypeObject.GetComponent<PrototypeInput>());

            var prototypeHealth = gameFactory.PrototypeObject.GetComponent<IHealth>();
            var prototypeEnergy = gameFactory.PrototypeObject.GetComponent<IEnergy>();
            this.prototypeHealth = gameFactory.PrototypeObject.GetComponent<PrototypeHealth>();

            if (this.prototypeHealth != null)
            {
                this.prototypeHealth.Healed += OnHealed;
                this.prototypeHealth.HealTimerUpdated += OnHealTimerUpdated;
            }

            if (window.HealthBar != null && prototypeHealth != null)
                window.HealthBar.SetResource(prototypeHealth);
            if (window.HealthText != null && prototypeHealth != null)
                window.HealthText.SetResource(prototypeHealth);

            if (window.EnergyBar != null && prototypeEnergy != null)
                window.EnergyBar.SetResource(prototypeEnergy);
            if (window.EnergyText != null && prototypeEnergy != null)
                window.EnergyText.SetResource(prototypeEnergy);
        }

        private void OnHealed()
        {
            if (prototypeHealth == null) return;

            window.LockImage.fillAmount = 1;
            prototypeHealth.Timer = prototypeHealth.HealCooldown;
        }

        private void OnHealTimerUpdated(float remainingTime)
        {
            if (window != null && window.LockImage != null)
            {
                float newFillAmount = remainingTime / prototypeHealth.HealCooldown;

                if (Mathf.Abs(window.LockImage.fillAmount - newFillAmount) > 0.01f)
                    window.LockImage.fillAmount = newFillAmount;
            }
        }

        public HUDWindow GetWindow() => window;

        public void CleanUp()
        {
            if (prototypeHealth != null)
                prototypeHealth.Healed -= OnHealed;
        }
    }
}