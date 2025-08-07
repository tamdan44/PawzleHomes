using System.Collections.Generic;

[UnityEngine.Scripting.Preserve]
[System.Serializable]
public class PlayerData
{
    public Dictionary<(int, int), int> playerLevelData; //value: -1: not unlocked, 0:unlocked, 1: 1 star, 2: 2 stars
    public bool[] stageUnlocked;
    public int playerBigCoins;  
    public int playerCoins;
    public int numHint;
    public bool adBlock;

    public PlayerData()
    {
        playerLevelData = GameData.playerLevelData;
        stageUnlocked = GameData.stageUnlocked;
        playerBigCoins = GameData.playerBigCoins;
        playerCoins = GameData.playerCoins;
        numHint = GameData.numHint;
        adBlock = GameData.AdBlock;
    }

}

