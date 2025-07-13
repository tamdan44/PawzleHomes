using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int stageID;
    public int levelID;
    public List<Vector3Int> tileIndices;
    public List<int> shapeDataIndices;
    public List<string> solutions;
    public string shapeColor;
    public LevelData()
    {
        stageID = 0;
        levelID = 0;
        tileIndices = new List<Vector3Int>();
        shapeDataIndices = new List<int>();
        solutions = new List<string>();
        shapeColor = "white";
    }
}

[System.Serializable]
public class LevelDatabase
{
    public List<LevelData> levels = new List<LevelData>();
}
