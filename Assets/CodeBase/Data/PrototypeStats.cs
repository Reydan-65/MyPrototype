using CodeBase.GamePlay;
using UnityEngine.Events;

namespace CodeBase.Data
{
    [System.Serializable]
    public class PrototypeStats
    {
        public event UnityAction Changed;

        public float MaxHitPoints;
        public float MaxEnergy;
        public float MovementSpeed;
        public float DashRange;
        public TurretType TurretType;
        public float ShootingRate;

        public static PrototypeStats stats = new PrototypeStats();

        public static PrototypeStats GetStats() => stats;
        public void IsChanged() => Changed?.Invoke();

        public static PrototypeStats GetDefaultStats()
        {
            stats.MaxHitPoints = 100;
            stats.MaxEnergy = 100;
            stats.MovementSpeed = 5;
            stats.DashRange = 6;
            stats.TurretType = TurretType.BulletLauncher;
            stats.ShootingRate = 0.5f;

            return stats;
        }

        public void SetDefaultStats()
        {
            MaxHitPoints = 100;
            MaxEnergy = 100;
            MovementSpeed = 5;
            DashRange = 6;
            TurretType = TurretType.BulletLauncher;
            ShootingRate = 0.5f;
        }

        public void CopyFrom(PrototypeStats data)
        {
            MaxHitPoints= data.MaxHitPoints;
            MaxEnergy= data.MaxEnergy;
            MovementSpeed = data.MovementSpeed;
            DashRange = data.DashRange;
            TurretType = data.TurretType;
            ShootingRate = data.ShootingRate;
        }

        public void AddStats(PrototypeStats bonusStats)
        {
            bool changed = false;

            if (bonusStats.MaxHitPoints != 0)
            {
                MaxHitPoints += bonusStats.MaxHitPoints;
                changed = true;
            }

            if (bonusStats.MaxEnergy != 0)
            {
                MaxEnergy += bonusStats.MaxEnergy;
                changed = true;
            }

            if (bonusStats.MovementSpeed != 0)
            {
                MovementSpeed += bonusStats.MovementSpeed;
                changed = true;
            }

            if (bonusStats.DashRange != 0)
            {
                DashRange += bonusStats.DashRange;
                changed = true;
            }

            if (bonusStats.ShootingRate != 0)
            {
                ShootingRate += bonusStats.ShootingRate;
                changed = true;
            }

            if (changed) Changed?.Invoke();
        }
    }
}
