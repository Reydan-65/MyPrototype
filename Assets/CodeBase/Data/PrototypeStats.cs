using CodeBase.GamePlay;
using UnityEngine.Events;

namespace CodeBase.Data
{
    

    [System.Serializable]
    public class PrototypeStats
    {
        public event UnityAction Changed;

        public StatItem Health = new StatItem(100f);
        public StatItem Energy = new StatItem(100f);
        public StatItem MovementSpeed = new StatItem(5f);
        public StatItem DashRange = new StatItem(6f);
        public StatItem ShootingRate = new StatItem(0.5f);

        public static PrototypeStats Default => new PrototypeStats()
        {
            Health = new StatItem(100f),
            Energy = new StatItem(100f),
            MovementSpeed = new StatItem(5f),
            DashRange = new StatItem(6f),
            ShootingRate = new StatItem(0.5f)
        };

        public void IsChanged() => Changed?.Invoke();

        public void SetDefaultStats()
        {
            Health.SetDefault(100f);
            Energy.SetDefault(100f);
            MovementSpeed.SetDefault(5f);
            DashRange.SetDefault(6f);
            ShootingRate.SetDefault(0.5f);
        }

        public void CopyFrom(PrototypeStats data)
        {
            Health.Level = data.Health.Level;
            Health.Value = data.Health.Value;

            Energy.Level = data.Energy.Level;
            Energy.Value = data.Energy.Value;

            MovementSpeed.Level = data.MovementSpeed.Level;
            MovementSpeed.Value = data.MovementSpeed.Value;

            DashRange.Level = data.DashRange.Level;
            DashRange.Value = data.DashRange.Value;

            ShootingRate.Level = data.ShootingRate.Level;
            ShootingRate.Value = data.ShootingRate.Value;
        }

        public static bool StatsAreEqual(PrototypeStats a, PrototypeStats b)
        {
            return a.Health.Value == b.Health.Value &&
                   a.Energy.Value == b.Energy.Value &&
                   a.MovementSpeed.Value == b.MovementSpeed.Value &&
                   a.DashRange.Value == b.DashRange.Value &&
                   a.ShootingRate.Value == b.ShootingRate.Value;
        }

        public int GetLevelForStat(string statName)
        {
            switch (statName)
            {
                case "HEALTH": return Health.Level;
                case "ENERGY": return Energy.Level;
                case "FIRE RATE": return ShootingRate.Level;
                case "SPEED": return MovementSpeed.Level;
                case "DASH RANGE": return DashRange.Level;
                default: return 1;
            }
        }

        public void SetLevelForStat(string statName, int level)
        {
            switch (statName)
            {
                case "HEALTH": Health.Level = level; break;
                case "ENERGY": Energy.Level = level; break;
                case "FIRE RATE": ShootingRate.Level = level; break;
                case "SPEED": MovementSpeed.Level = level; break;
                case "DASH RANGE": DashRange.Level = level; break;
            }
        }
    }
}
