using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class NativeOverlayAds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            Debug.Log("MobileAds SDK is initialized");
            LoadAd();
        });

        if (_nativeOverlayAd != null)
        {
            RenderAd();
            ShowAd();
        }
    }

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/3986624511";
#else
    private string _adUnitId = "unused";
#endif


    private NativeOverlayAd _nativeOverlayAd;

    /// <summary>
    /// Loads the ad.
    /// </summary>
    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (_nativeOverlayAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading native overlay ad.");

        // Create a request used to load the ad.
        var adRequest = new AdRequest();

        // Optional: Define native ad options.
        var options = new NativeAdOptions
        {
            AdChoicesPlacement = AdChoicesPlacement.TopRightCorner,
            MediaAspectRatio = MediaAspectRatio.Any,
        };

        // Send the request to load the ad.
        NativeOverlayAd.Load(_adUnitId, adRequest, options,
            (NativeOverlayAd ad, LoadAdError error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Native Overlay ad failed to load an ad " +
                               " with error: " + error);
                    return;
                }

                // The ad should always be non-null if the error is null, but
                // double-check to avoid a crash.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Native Overlay ad load event " +
                               " fired with null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Native Overlay ad loaded with response : " +
                           ad.GetResponseInfo());
                _nativeOverlayAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);
            });
    }

    /// <summary>
    /// Renders the ad.
    /// </summary>
    public void RenderAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Rendering Native Overlay ad.");

            // Define a native template style with a custom style.
            var style = new NativeTemplateStyle
            {
                TemplateId = NativeTemplateId.Medium,
                MainBackgroundColor = Color.red,
                CallToActionText = new NativeTemplateTextStyle
                {
                    BackgroundColor = Color.green,
                    TextColor = Color.white,
                    FontSize = 9,
                    Style = NativeTemplateFontStyle.Bold
                }
            };

            // Renders a native overlay ad at the default size
            // and anchored to the bottom of the screne.
            _nativeOverlayAd.RenderTemplate(style, AdPosition.Bottom);
        }
    }

    public void ShowAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Showing Native Overlay ad.");
            _nativeOverlayAd.Show();
        }
    }

    public void HideAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Hiding Native Overlay ad.");
            _nativeOverlayAd.Hide();
        }
    }

    public void DestroyAd()
    {
        if (_nativeOverlayAd != null)
        {
            Debug.Log("Destroying native overlay ad.");
            _nativeOverlayAd.Destroy();
            _nativeOverlayAd = null;
        }
    }

    private void RegisterEventHandlers(NativeOverlayAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Native Overlay ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Native Overlay ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Native Overlay ad was clicked.");
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Native Overlay ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Native Overlay ad full screen content closed.");
        };
    }
}
