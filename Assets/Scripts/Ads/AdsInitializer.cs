using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private bool testMode = true;
    [SerializeField] private string androidAdId = "4886440";
    [SerializeField] private string iOSAdId = "4886441";
    private string gameId;
    [SerializeField] private GameObject adsController;

    void Awake()
    {
        if (PlayerPrefs.GetInt("removeads", 0) == 1)
        {
            Debug.Log("Ads Removed");
            adsController.SetActive(false);
            return;
        }
        InitializeAds();
    }

    public void InitializeAds()
    {
        Debug.Log("InitializeAds");
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdId : androidAdId;
        Advertisement.Initialize(gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("OnInitializationComplete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("OnInitializationFailed");
    }
}
