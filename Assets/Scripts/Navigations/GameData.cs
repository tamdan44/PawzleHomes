

using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    // public static Vector3[,,] GridTilePosition;
    public static bool[] onBoardShapes;

    public static int currentStage;
    public static int currentLevel;
    public static List<Vector3Int> tileIndices;
    public static List<int> shapeDataIndices;
    public static List<string> solutions;
    public static string shapeColor;

}
