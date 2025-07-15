using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
    public class AdsService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IAdsService
    {
        private const string AndroidGameID = "5892161";
        private const string IOSGameID = "5892160";
        private const string InterstitialAndroid = "Interstitial_Android";
        private const string InterstitialIOS = "Interstitial_IOS";
        private const string RewardedAndroid = "Rewarded_Android";
        private const string RewardedIOS = "Rewarded_IOS";
        private const bool TestMode = true;

        private string interstitialID;
        private string rewardedID;
        //private string gameID;

        private bool isRewardedVideoReady;
        public bool IsRewardedVideoReady => isRewardedVideoReady;

        private event UnityAction rewardedVideoCompleted;

        public void Initialize()
        {
#if UNITY_IOS
            interstitialID = InterstitialIOS;
            rewardedID = RewardedIOS;
            gameID = IOSGameID;
#elif UNITY_ANDROID
            interstitialID = InterstitialAndroid;
            rewardedID = RewardedAndroid;
            gameID = AndroidGameID;
#elif UNITY_EDITOR
            interstitialID = InterstitialAndroid;
            rewardedID = RewardedAndroid;
            //gameID = AndroidGameID;
            isRewardedVideoReady = true;
#endif

            if (Advertisement.isInitialized == false && Advertisement.isSupported == true)
            {
                Advertisement.Initialize(AndroidGameID, TestMode, this);
            }
        }

        public void LoadInterstitial()
        {
            Advertisement.Load(interstitialID, this);
        }

        public void ShowInterstitial()
        {
            Advertisement.Show(interstitialID, this);
        }

        public void LoadRewarded()
        {
            Advertisement.Load(rewardedID, this);
        }

        public void ShowRewarded(UnityAction videoCompleted)
        {
            Advertisement.Show(rewardedID, this);
            rewardedVideoCompleted = videoCompleted;
        }

        // IUnityAdsInitializationListener
        public void OnInitializationComplete()
        {
            Debug.Log("OnInitializationComplete");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log("OnInitializationFailed");
        }

        // IUnityAdsLoadListener
        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("OnUnityAdsAdLoaded");

            if (placementId == rewardedID)
            {
                isRewardedVideoReady = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log("OnUnityAdsFailedToLoad");
        }

        // IUnityAdsShowListener
        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log("OnUnityAdsShowClick");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId == rewardedID)
            {
                if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
                {
                    rewardedVideoCompleted?.Invoke();
                }

                rewardedVideoCompleted = null;
            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log("OnUnityAdsShowFailure");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log("OnUnityAdsShowStart");
        }
    }
}
