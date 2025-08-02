using CodeBase.Infrastructure.Services.Factory;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;

namespace CodeBase.GamePlay.UI
{
    public class HUDPresenter : WindowPresenterBase<HUDWindow>
    {
        private readonly IGameFactory gameFactory;
        private HUDWindow window;

        public HUDPresenter(IGameFactory gameFactory)
        {
            this.gameFactory = gameFactory;
            this.gameFactory.HeroCreated += OnHeroCreated;
        }

        public override void SetWindow(HUDWindow window)
        {
            this.window = window;
            UpdateResourcesDisplay();
        }

        private void OnHeroCreated()
        {
            UpdateResourcesDisplay();
        }

        private void UpdateResourcesDisplay()
        {
            if (window == null || gameFactory.HeroObject == null)
                return;

            var heroHealth = gameFactory.HeroObject.GetComponent<IHealth>();
            var heroEnergy = gameFactory.HeroObject.GetComponent<IEnergy>();

            if (window.HealthBar != null && heroHealth != null)
                window.HealthBar.SetResource(heroHealth);
            if (window.HealthText != null && heroHealth != null)
                window.HealthText.SetResource(heroHealth);

            if (window.EnergyBar != null && heroEnergy != null)
                window.EnergyBar.SetResource(heroEnergy);
            if (window.EnergyText != null && heroEnergy != null)
                window.EnergyText.SetResource(heroEnergy);
        }

        public HUDWindow GetWindow() => window;

        public void CleanUp()
        {
            if (gameFactory != null)
                gameFactory.HeroCreated -= OnHeroCreated;
        }
    }
}