using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[System.Serializable]
public class PlayerLevelData
{
    public int stageID;
    public int levelID;
    public int status = -1; //-1: not unlocked, 0:unlocked, 1: 1 star, 2: 2 stars
    
    public PlayerLevelData()
    {
    }
}

[System.Serializable]
public class PlayerLevelDatabase
{
    public List<PlayerLevelData> levels = new List<PlayerLevelData>();
}
