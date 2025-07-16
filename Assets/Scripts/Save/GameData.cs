

using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
     #region PlayerItems
    public static int playerBigCoins;
    public static int playerCoins;
    public static int numHint;
    #endregion


    #region PlayerLevels
    public static Dictionary<(int, int), int> playerLevelData;
    public static bool[] stageUnlocked;
    #endregion


    #region CurrentPuzzle
    // do not save
    public static bool[] onBoardShapes;

    public static int currentStage;
    public static int currentLevel;
    public static List<Vector3Int> tileIndices;
    public static List<int> shapeDataIndices;
    public static List<string> solutions;
    public static string shapeColor;
    public static LevelDatabase levelDB;
    #endregion
}
