using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class RewardedInterstitialAds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log("MobileAds SDK is initialized");
            LoadRewardedInterstitialAd();
        });

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            ShowRewardedInterstitialAd();
        }
    }

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
    private string _adUnitId = "unused";
    //private string _adUnitId = "ca-app-pub-3940256099942544/5354046379";
#endif

    private RewardedInterstitialAd rewardedInterstitialAd;

    /// <summary>
    /// Loads the rewarded interstitial ad.
    /// </summary>
    public void LoadRewardedInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedInterstitialAd != null)
        {
            DestroyAd();
            rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedInterstitialAd.Load(_adUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedInterstitialAd = ad;

                RegisterEventHandlers(rewardedInterstitialAd);
                RegisterReloadHandler(rewardedInterstitialAd);
            });
    }

    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    public void DestroyAd()
    {
        if (rewardedInterstitialAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }

        // Inform the UI that the ad is not ready.
        //AdLoadedStatus?.SetActive(false);
    }

    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);
        };
    }

    private void RegisterReloadHandler(RewardedInterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
    {
            Debug.Log("Rewarded interstitial ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedInterstitialAd();
        };
    }
}
