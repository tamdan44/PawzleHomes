using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int stageID;
    public int levelID;
    public List<Vector3Int> tileIndices;
    public List<int> shapeDataIndices;
    public List<string> solutions;
    
}

[System.Serializable]
public class LevelDatabase
{
    public List<LevelData> levels = new List<LevelData>();
}
