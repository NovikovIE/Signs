using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Advertisements;

public class Purchaser : MonoBehaviour
{
    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "signs.removeads":
                RemoveAds();
                break;
        }
    }

    private void RemoveAds()
    {
        PlayerPrefs.SetInt("removeads", 1);
        Debug.Log("removeads");
        UIInfo.Instance.UpdateRemoveAdsButton();
        try { Advertisement.Banner.Hide(); }
        catch { }
    }
}
