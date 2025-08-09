using CodeBase.GamePlay.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace CodeBase.Data
{
    [System.Serializable]
    public class ProjectileStats
    {
        public event UnityAction Changed;

        public List<ProjectileType> TypeKeys = new List<ProjectileType>();
        public List<ProjectileTypeStats> TypeValues = new List<ProjectileTypeStats>();

        public Dictionary<ProjectileType, ProjectileTypeStats> TypeStats
        {
            get
            {
                var dictionary = new Dictionary<ProjectileType, ProjectileTypeStats>();
                for (int i = 0; i < TypeKeys.Count; i++)
                    if (i < TypeValues.Count)
                        dictionary[TypeKeys[i]] = TypeValues[i];
                return dictionary;
            }
            set
            {
                TypeKeys.Clear();
                TypeValues.Clear();

                foreach (var item in value)
                {
                    TypeKeys.Add(item.Key);
                    TypeValues.Add(item.Value);
                }
            }
        }

        public static ProjectileStats Default => new ProjectileStats()
        {
            TypeStats = new Dictionary<ProjectileType, ProjectileTypeStats>()
            {
                [ProjectileType.Bullet] = new ProjectileTypeStats()
                {
                    AverageDamage = new StatItem(15f),
                    Speed = new StatItem(10f),
                    Range = new StatItem(7f)
                },
                [ProjectileType.Rocket] = new ProjectileTypeStats()
                {
                    AverageDamage = new StatItem(25f),
                    Speed = new StatItem(7f),
                    Range = new StatItem(7f)
                }
            }
        };

        public void IsChanged() => Changed?.Invoke();

        public ProjectileStats()
        {
            foreach (ProjectileType type in System.Enum.GetValues(typeof(ProjectileType)))
            {
                var stats = new ProjectileTypeStats();
                TypeKeys.Add(type);
                TypeValues.Add(stats);
            }
        }

        public ProjectileTypeStats GetStatsForType(ProjectileType type)
        {
            if (TypeStats.TryGetValue(type, out var stats)) return stats;
            return new ProjectileTypeStats();
        }

        public void CopyFrom(ProjectileStats data)
        {
            if (data == null) return;

            TypeKeys = new List<ProjectileType>(data.TypeKeys);
            TypeValues = new List<ProjectileTypeStats>();

            foreach (var stats in data.TypeValues)
            {
                var newStats = new ProjectileTypeStats();
                newStats.CopyFrom(stats);
                TypeValues.Add(newStats);
            }
        }

        public static bool StatsAreEqual(ProjectileStats a, ProjectileStats b)
        {
            if (a == null || b == null) return false;

            var aStats = a.TypeStats;
            var bStats = b.TypeStats;

           UnityEngine.Debug.Log("=== Comparing ProjectileStats ===");
            UnityEngine.Debug.Log("A Stats: " + string.Join(", ", a.TypeStats.Select(x => $"{x.Key}:{x.Value.AverageDamage.Value}")));
            UnityEngine.Debug.Log("B Stats: " + string.Join(", ", b.TypeStats.Select(x => $"{x.Key}:{x.Value.AverageDamage.Value}")));

            if (aStats.Count != bStats.Count) return false;

            foreach (var kvp in aStats)
            {
                if (!bStats.TryGetValue(kvp.Key, out var bValue)) return false;
                if (!ProjectileTypeStats.StatsAreEqual(kvp.Value, bValue)) return false;
            }

            return true;
        }

        public int GetLevelForStat(ProjectileType type, string statName)
        {
            switch (statName)
            {
                case "DAMAGE": return TypeStats[type].AverageDamage.Level;
                case "SPEED": return TypeStats[type].Speed.Level;
                case "RANGE": return TypeStats[type].Range.Level;
                default: return 1;
            }
        }
    }
}