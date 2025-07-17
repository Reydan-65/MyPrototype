using CodeBase.GamePlay.Turrets;
using UnityEngine.Events;

namespace CodeBase.Data
{
    [System.Serializable]
    public class HeroStats
    {
        public event UnityAction Changed;

        public float MaxHitPoints;
        public float MovementSpeed;
        public float DashRange;
        public TurretType TurretType;
        public float ShootingRate;

        public static HeroStats stats = new HeroStats();

        public static HeroStats GetDefaultStats()
        {
            stats.MaxHitPoints = 100;
            stats.MovementSpeed = 5;
            stats.DashRange = 50;
            stats.TurretType = TurretType.BulletLauncher;
            stats.ShootingRate = 0.5f;

            return stats;
        }

        public void SetDefaultStats()
        {
            MaxHitPoints = 100;
            MovementSpeed = 5;
            DashRange = 50;
            TurretType = TurretType.BulletLauncher;
            ShootingRate = 0.5f;
        }

        public static HeroStats GetStats()
        {
            return stats;
        }

        public void CopyFrom(HeroStats data)
        {
            MaxHitPoints= data.MaxHitPoints;
            MovementSpeed = data.MovementSpeed;
            DashRange = data.DashRange;
            TurretType = data.TurretType;
            ShootingRate = data.ShootingRate;
        }

        public void AddStats(HeroStats bonusStats)
        {
            bool changed = false;

            if (bonusStats.MaxHitPoints != 0)
            {
                MaxHitPoints += bonusStats.MaxHitPoints;
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

        public void IsChanged()
        {
            Changed?.Invoke();
        }
    }
}
