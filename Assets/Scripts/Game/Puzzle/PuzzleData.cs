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

}

[System.Serializable]
public class LevelDatabase
{
    public List<LevelData> levels = new List<LevelData>();
}
