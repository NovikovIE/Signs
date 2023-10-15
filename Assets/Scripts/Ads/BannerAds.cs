using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] private string androidAdID = "Banner_Android";
    [SerializeField] private string iOSAdID = "Banner_iOS";

    private string adId;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        adId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSAdID
            : androidAdID;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        if (PlayerPrefs.GetInt("removeads", 0) == 1) return;
        Advertisement.Banner.SetPosition(bannerPosition);
        StartCoroutine(LoadAdBanner());
    }

    private IEnumerator LoadAdBanner()
    {
        yield return new WaitForSeconds(1f);
        LoadBanner();
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(adId, options);
    }

    private void OnBannerLoaded()
    {
        Debug.Log("OnBannerLoaded");
        ShowBannerAd();
    }

    private void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.Show(adId, options);
    }

    private void OnBannerError(string message)
    {
        Debug.Log($"OnBannerError: {message}");
    }

    private void OnBannerClicked() { }
    private void OnBannerHidden() { }
    private void OnBannerShown() { }
}
