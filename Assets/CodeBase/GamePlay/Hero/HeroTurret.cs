using CodeBase.Data;
using CodeBase.GamePlay.Turrets;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;

namespace CodeBase.GamePlay.Hero
{
    public class HeroTurret : Turret, IProgressLoadHandler
    {
        private IInputService inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            this.inputService = inputService;
        }

        protected override void Update()
        {
            base.Update();
        
            if (inputService != null && CanFire()) Fire();
        }

        protected override bool CanFire() => timer >= shootingRate && inputService.AttackInput;

        public void LoadProgress(PlayerProgress progress)
        {
            turretType = progress.HeroStats.TurretType;
            shootingRate = progress.HeroStats.ShootingRate;
        }
    }
}