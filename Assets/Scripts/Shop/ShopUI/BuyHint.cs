using UnityEngine;
using TMPro;

public class BuyHint : MonoBehaviour
{
    public MoneyBar moneyBar;
    [SerializeField] private loadRewarded RewardedAd;

    // [SerializeField] private RewardedAdPanel rewardedAdPanel;
    void OnEnable()
    {
        moneyBar.gameObject.SetActive(true);
    }
    void OnDisable()
    {
        moneyBar.gameObject.SetActive(false);
    }

    public void BuyHintClicked()
    {
        if (GameData.playerCoins >= 180)
        {
            GameData.numHint++;
            GameEvents.AddCoins(-180);
            AudioManager.instance.PlayGlobalSFX("coin-reward");
        }
        else
            AudioManager.instance.PlayGlobalSFX("cat-sad");
    }

    public void WatchAdClicked()
    {
        RewardedAd.LoadAd();
        moneyBar.AddCoins(500);
        // CloseButtonClicked();
    }

    public void WatchAdForStarClicked()
    {
        RewardedAd.LoadAd();
        moneyBar.AddBigCoins(50);
        // CloseButtonClicked();
    }
    public void CloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
