using CodeBase.Data;
using CodeBase.Infrastructure.DependencyInjection;
using CodeBase.Infrastructure.Services.PlayerProgressProvider;
using UnityEngine;

namespace CodeBase.GamePlay.Prototype
{
    public class PrototypeStatsViewer : MonoBehaviour
    {
        public PrototypeStats prototypeStats;

        private IProgressProvider progressProvider;

        [Inject]
        public void Construct(IProgressProvider progressProvider)
        {
            this.progressProvider = progressProvider;
            if (this.progressProvider != null)
                this.progressProvider.PlayerProgress.PrototypeStats.Changed += OnStatsChanged;
            prototypeStats = progressProvider.PlayerProgress.PrototypeStats;
        }

        private void OnStatsChanged() => prototypeStats = progressProvider.PlayerProgress.PrototypeStats;
        private void OnDestroy() => progressProvider.PlayerProgress.PrototypeStats.Changed -= OnStatsChanged;
    }
}