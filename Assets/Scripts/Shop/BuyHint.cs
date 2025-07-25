using UnityEngine;

public class BuyHint : MonoBehaviour
{
    public MoneyBar moneyBar;
    [SerializeField] private RewardedAdPanel rewardedAdPanel;
    void Start()
    {

    }

    void AddCoins(int coins)
    {
        GameData.playerCoins += coins;
        SaveSystem.SavePlayer();
            moneyBar.UpdateCoinNum();
    }

    public void BuyHintClicked()
    {
        if (GameData.playerCoins >= 180)
        {
            AddCoins(-180);
            GameData.numHint += 1;
        }
        else
        {
            rewardedAdPanel.gameObject.SetActive(true);
            if (rewardedAdPanel.adWatched)
            {
                 AddCoins(500);
                rewardedAdPanel.adWatched = false;
            }
           
        }
    }

    public void CloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
