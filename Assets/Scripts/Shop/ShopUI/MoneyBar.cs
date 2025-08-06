using UnityEngine;
using TMPro;

public class MoneyBar : MonoBehaviour
{
    [SerializeField] private TMP_Text numCoin;
    [SerializeField] private TMP_Text numBigCoin;

    void Start()
    {
        numCoin.text = GameData.playerCoins.ToString();
        numBigCoin.text = GameData.playerBigCoins.ToString();
    }

    public void UpdateCoinNum()
    {
        numCoin.text = GameData.playerCoins.ToString();
        numBigCoin.text = GameData.playerBigCoins.ToString();
    }

    public void AddCoins(int coins)
    {
        GameData.playerCoins += coins;
        SaveSystem.SavePlayer();
        UpdateCoinNum();
    }


}
