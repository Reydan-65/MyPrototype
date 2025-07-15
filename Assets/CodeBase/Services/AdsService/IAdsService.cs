using CodeBase.Infrastructure.DependencyInjection;
using UnityEngine.Events;

namespace CodeBase.Infrastructure.Services
{
    public interface IAdsService : IService
    {
        void Initialize();
        void LoadInterstitial();
        void LoadRewarded();
        void ShowInterstitial();
        void ShowRewarded(UnityAction videoCompleted);

        bool IsRewardedVideoReady {  get; }
    }
}