using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;

public class RewardedAdPanel : MonoBehaviour
{
    public MoneyBar moneyBar;
    [SerializeField] private loadRewarded RewardedAd;
    [SerializeField] private TMP_Text panelText;
    // public bool adWatched { get; set; }

    void Start()
    {
    }
    
    public void YesClicked()
    {
        RewardedAd.LoadAd();
        moneyBar.AddCoins(500);
        NoClicked();
    }

    public void NoClicked()
    {
        gameObject.SetActive(false);
    }


}
