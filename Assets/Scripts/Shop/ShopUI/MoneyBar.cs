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

    void OnEnable()
    {
        GameEvents.AddCoins += AddCoins;
        GameEvents.AddBigCoins += AddBigCoins;
    }

    void OnDisable()
    {
        GameEvents.AddCoins -= AddCoins;
        GameEvents.AddBigCoins -= AddBigCoins;
    }

    public void UpdateCoinNum()
    {
        numCoin.text = GameData.playerCoins.ToString();
        numBigCoin.text = GameData.playerBigCoins.ToString();
    }

    public void AddCoins(int coins)
    {
        GameData.playerCoins += coins;
        GameEvents.SavePlayer();
        UpdateCoinNum();
    }

    public void AddBigCoins(int bigcoins)
    {
        GameData.playerBigCoins += bigcoins;
        GameEvents.SavePlayer();
        UpdateCoinNum();
    }


}
