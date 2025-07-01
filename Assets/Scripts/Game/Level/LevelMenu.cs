using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class LevelMenu : MonoBehaviour
{
    private LevelDatabase levelDB;
    private const string filePath = "Assets/Resources/levels.json";

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

    }

    public void OpenLevel(string text)
    {

        var match = Regex.Match(text, @"stage(?<stageID>\d+)\s+level(?<levelID>\d+)");
        if (match.Success)
        {
            int stageID = int.Parse(match.Groups["stageID"].Value);
            int levelID = int.Parse(match.Groups["levelID"].Value);

            Debug.Log($"Stage ID: {stageID}");
            Debug.Log($"Level ID: {levelID}");

            GameData.currentStage = stageID;
            GameData.currentLevel = levelID;

            SceneManager.LoadScene("Play");
        }
        else
        {
            Debug.Log("Input string format is invalid.");
        }

        foreach (var level in levelDB.levels)
        {
            if (level.stageID == GameData.currentStage && level.levelID == GameData.currentLevel)
            {
                GameData.tileIndices = level.tileIndices;
                GameData.shapeDataIndices = level.shapeDataIndices;
            }
        }


    }
}
