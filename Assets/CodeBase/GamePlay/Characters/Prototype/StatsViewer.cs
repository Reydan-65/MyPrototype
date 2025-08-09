using CodeBase.Data;
using CodeBase.GamePlay.Projectile;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class StatsViewer : MonoBehaviour
    {
        [Serializable]
        public class ProjectileStatsDisplay
        {
            public ProjectileType Type;
            public StatItem AverageDamage;
            public StatItem Speed;
            public StatItem Range;
        }

        [Header("Prototype Stats")]
        public PrototypeStats PrototypeStats;

        [Header("Projectile Stats")]
        public List<ProjectileStatsDisplay> ProjectilesStats = new List<ProjectileStatsDisplay>();

        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;

            if (this.progressProvider.PlayerProgress != null)
            {
                SubscribeToEvents();
                UpdateStatsDisplay();
            }
        }

        private void SubscribeToEvents()
        {
            progressProvider.PlayerProgress.PrototypeStats.Changed += OnStatsChanged;
            progressProvider.PlayerProgress.ProjectileStats.Changed += OnProjectileStatsChanged;
        }

        private void UnsubscribeFromEvents()
        {
            if (progressProvider?.PlayerProgress != null)
            {
                progressProvider.PlayerProgress.PrototypeStats.Changed -= OnStatsChanged;
                progressProvider.PlayerProgress.ProjectileStats.Changed -= OnProjectileStatsChanged;
            }
        }

        private void UpdateStatsDisplay()
        {
            UpdatePrototypeStats();
            UpdateProjectileStats();
        }

        private void UpdatePrototypeStats()
        {
            PrototypeStats = progressProvider.PlayerProgress.PrototypeStats;
        }

        private void UpdateProjectileStats()
        {
            ProjectilesStats.Clear();

            var stats = progressProvider.PlayerProgress.ProjectileStats.TypeStats;

            foreach (var kvp in stats)
            {
                ProjectilesStats.Add(new ProjectileStatsDisplay
                {
                    Type = kvp.Key,
                    AverageDamage = new StatItem(kvp.Value.AverageDamage.Value)
                    {
                        Level = kvp.Value.AverageDamage.Level
                    },
                    Speed = new StatItem(kvp.Value.Speed.Value)
                    {
                        Level = kvp.Value.Speed.Level
                    },
                    Range = new StatItem(kvp.Value.Range.Value)
                    {
                        Level = kvp.Value.Range.Level
                    }
                });
            }
        }

        private void OnStatsChanged() => UpdatePrototypeStats();
        private void OnProjectileStatsChanged() => UpdateProjectileStats();
        private void OnDestroy() => UnsubscribeFromEvents();
    }
}