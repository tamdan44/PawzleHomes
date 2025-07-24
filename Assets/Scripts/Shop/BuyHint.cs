using UnityEngine;

public class BuyHint : MonoBehaviour
{
    [SerializeField] public MoneyBar moneyBar;
    void Start()
    {
        
    }

    void AddCoins(int coins)
    {
        GameData.playerCoins += coins;
    }

    public void HintBtnClicked()
    {
        if (GameData.playerCoins >= 180)
        {
            AddCoins(-180);
            GameData.numHint += 1;
            moneyBar.UpdateCoinNum();
            SaveSystem.SavePlayer();
        }
    }
}
