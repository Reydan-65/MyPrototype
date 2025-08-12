namespace CodeBase.Data
{
    [System.Serializable]
    public class ProjectileTypeStats
    {
        public StatItem AverageDamage = new StatItem(15f);
        public StatItem Speed = new StatItem(10f);
        public StatItem Range = new StatItem(6f);

        public void SetDefaults(float baseDamage, float baseSpeed, float baseRange)
        {
            AverageDamage.SetDefault(baseDamage);
            Speed.SetDefault(baseSpeed);
            Range.SetDefault(baseRange);
        }

        public void CopyFrom(ProjectileTypeStats data)
        {
            if (data == null) return;

            AverageDamage = new StatItem(data.AverageDamage.Value) { Level = data.AverageDamage.Level };
            Speed = new StatItem(data.Speed.Value) { Level = data.Speed.Level };
            Range = new StatItem(data.Range.Value) { Level = data.Range.Level };
        }

        public static bool StatsAreEqual(ProjectileTypeStats a, ProjectileTypeStats b)
        {
            if (a == null || b == null) return false;

            return a.AverageDamage.Level == b.AverageDamage.Level &&
                   a.Speed.Level == b.Speed.Level &&
                   a.Range.Level == b.Range.Level;
        }
    }
}