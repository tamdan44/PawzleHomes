using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ShapeStorage shapeStorage;


    [HideInInspector]
    private LevelDatabase levelDB;
    
    private const string filePath = "Assets/Resources/levels.json";
    private List<string> solutions;
    void Start()
    {
        // Load existing file if it exists
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            levelDB = JsonUtility.FromJson<LevelDatabase>(json);
            Debug.Log($"File.Exists {filePath}");
        }
        else
        {
            levelDB = new LevelDatabase(); // create new if file doesn't exist
            Debug.Log($"File not exist");
        }

        Dictionary<int, List<Vector3Int>> shapeCurrentPositions = gridManager.shapeCurrentPositions;
        for (int i = 0; i < shapeStorage.shapeList.Count; i++) {
            
            if (shapeCurrentPositions[i] != null)
            {
            }
        }
        
    }

    public void SaveLevelJson()
    {
        string updatedJson = JsonUtility.ToJson(levelDB, true);
        File.WriteAllText(filePath, updatedJson);
        Debug.Log("Added level and saved to " + filePath);
    }

    public void SpawnShapes()
    {
        foreach (Shape shape in shapeStorage.shapeList)
        {
            shape._isActive = true;
        }
        GameEvents.RequestNewShapes();
    }

    public void ClearThisGrid(){
        GameEvents.ClearGrid();
    }

    bool IsDuplicatedLevel(int stageID, int levelID) =>
        levelDB.levels.Any(l => l.stageID == stageID && l.levelID == levelID);

    void RemoveLevel(int stageID, int levelID)
    {
        levelDB.levels.RemoveAll(l => l.stageID == stageID && l.levelID == levelID);
    }

    public void AddLevel()
    {
        Debug.Log($"AddLevel");
        int stageID = 0;
        int levelID = 0;

        Text buttonText = this.GetComponentInChildren<Text>();
        var match = Regex.Match(buttonText.text, @"stage(?<stageID>\d+)\s+level(?<levelID>\d+)");
        if (match.Success)
        {
            stageID = int.Parse(match.Groups["stageID"].Value);
            levelID = int.Parse(match.Groups["levelID"].Value);
        }
        else
        {
            Debug.Log("Input string format is invalid.");
        }

        RemoveLevel(stageID, levelID);


        LevelData newLevel = new LevelData
        {
            stageID = stageID,
            levelID = levelID,
            tileIndices = gridManager.GetVisibleTiles(),
            shapeDataIndices = shapeStorage.GetCurrentShapeDatas()
        };

        levelDB.levels.Add(newLevel);
    }  
}
