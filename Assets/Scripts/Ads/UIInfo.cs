using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo : MonoBehaviour
{
    public static UIInfo Instance;
    [SerializeField] GameObject removeAdsButton;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Instance = this;
        UpdateRemoveAdsButton();
    }

    public void UpdateRemoveAdsButton()
    {
        bool removeAds = PlayerPrefs.GetInt("removeads") == 1;
        removeAdsButton.SetActive(!removeAds);
    }
}
