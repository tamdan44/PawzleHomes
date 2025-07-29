using UnityEngine;

public class BuyHint : MonoBehaviour
{
    public MoneyBar moneyBar;
    [SerializeField] private RewardedAdPanel rewardedAdPanel;
    void Start()
    {

    }

    public void BuyHintClicked()
    {
        if (GameData.playerCoins >= 180)
        {
            GameData.numHint++;
            moneyBar.AddCoins(-180);
            Debug.Log(GameData.numHint);
        }
        else
        {
            rewardedAdPanel.gameObject.SetActive(true);
        }
    }


    public void CloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
