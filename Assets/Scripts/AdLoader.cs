using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
// using UnityEngine.UI; // Comment out this line since we're using TextMeshPro
using TMPro; // Add this for TextMeshPro support

public class AdRewardManager : MonoBehaviour
{
    // Reference to the UI Text to display money
    // public Text moneyText; // Comment out this line
    public TMP_Text moneyText; // Use TMP_Text for TextMeshPro

    private int playerMoney = 0; // Initial money value
    private const int rewardAmount = 5; // Amount to reward per ad

    private RewardedAd _rewardedAd;

    // Ad Unit IDs for rewarded ads
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-9742491029549202~6192666340";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-9742491029549202~6192666340";
#else
    private string _adUnitId = "unused";
#endif

    void Start()
    {
        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob SDK Initialized");
        });

        // Load initial money from PlayerPrefs
        playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        UpdateMoneyUI();

        // Load the rewarded ad
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // Create our request used to load the ad
        var adRequest = new AdRequest();

        // Send the request to load the ad
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response: " + ad.GetResponseInfo());
                _rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                // Reward the user by increasing money
                playerMoney += rewardAmount;
                PlayerPrefs.SetInt("PlayerMoney", playerMoney);
                PlayerPrefs.Save();
                UpdateMoneyUI();
                RegisterReloadHandler(_rewardedAd);
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error: " + error);
            LoadRewardedAd();
        };
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = playerMoney + "$";
        }
        else
        {
            Debug.LogWarning("MoneyText reference is not set in the Inspector!");
        }
    }

    void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
        }
    }
}