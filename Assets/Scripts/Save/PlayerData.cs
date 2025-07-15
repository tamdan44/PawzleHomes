using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Dictionary<(int, int), int> playerLevelData; //value: -1: not unlocked, 0:unlocked, 1: 1 star, 2: 2 stars
    public bool[] stageUnlocked;
    public PlayerData()
    {
        playerLevelData = GameData.playerLevelData;
        stageUnlocked = GameData.stageUnlocked;
    }

}

[System.Serializable]
public class PlayerLevelDatabase
{
    public List<PlayerData> levels = new List<PlayerData>();
}
