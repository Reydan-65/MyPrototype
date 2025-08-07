using CodeBase.Data;
using CodeBase.Infrastructure.Services.PlayerProgressSaver;
using System;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeTurret : Turret, IProgressLoadHandler
    {
        public event Action<bool> AimingStateChanged;

        private bool isAiming;
        public bool IsAiming
        {
            get => isAiming;
            set
            {
                if (isAiming != value)
                {
                    isAiming = value;
                    AimingStateChanged?.Invoke(isAiming);
                }
            }
        }

        public void Initialize(float value)
        {
            shootingRate = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            Initialize(progress.PrototypeStats.ShootingRate.Value);
        }
    }
}