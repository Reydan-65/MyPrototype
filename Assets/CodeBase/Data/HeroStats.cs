using CodeBase.GamePlay.Turrets;

namespace CodeBase.Data
{
    [System.Serializable]
    public class HeroStats
    {
        public int Damage;
        public float MaxHitPoints;
        public float MovementSpeed;
        public float DashRange;
        public TurretType TurretType;
        public float ShootingRate;

        public static HeroStats stats = new HeroStats();

        public static HeroStats GetDefaultStats()
        {
            stats.Damage = 60;
            stats.MaxHitPoints = 100;
            stats.MovementSpeed = 5;
            stats.DashRange = 100;
            stats.TurretType = TurretType.BulletLauncher;
            stats.ShootingRate = 0.5f;

            return stats;
        }

        public void SetDefaultStats()
        {
            Damage = 60;
            MaxHitPoints = 100;
            MovementSpeed = 5;
            DashRange = 100;
            TurretType = TurretType.BulletLauncher;
            ShootingRate = 0.5f;
        }

        public static HeroStats GetStats()
        {
            return stats;
        }

        public void CopyFrom(HeroStats data)
        {
            Damage = data.Damage;
            MaxHitPoints= data.MaxHitPoints;
            MovementSpeed = data.MovementSpeed;
            DashRange = data.DashRange;
            TurretType = data.TurretType;
            ShootingRate = data.ShootingRate;
        }
    }
}
