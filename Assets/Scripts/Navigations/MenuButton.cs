using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuButtons : MonoBehaviour
{
    private const string filePath = "Assets/Resources/levels.json";
    void Awake()
    {
        if (!Application.isEditor)
        {
            Debug.unityLogger.logEnabled = false;
        }
        // Load existing file if it exists
        if (File.Exists(filePath) && GameData.levelDB==null)
        {
            string json = File.ReadAllText(filePath);
            GameData.levelDB = JsonUtility.FromJson<LevelDatabase>(json);
            Debug.Log($"File.Exists {filePath}");
        }
        else
        {
            Debug.Log($"File not exist");
        }


    }
    void OnEnable()
    {
        GameEvents.OpenLevel += OpenLevel;
    }
    void OnDisable()
    {
        // GameEvents.OpenLevel -= OpenLevel;
    }

    public void LoadScreen(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadNextLevel()
    {
        GameData.currentLevel += 1;
        GameEvents.OpenLevel(GameData.currentStage, GameData.currentLevel);
    }
    public void OpenLevel(int stageID, int levelID) // fix this to make it load 
    {

        // var match = Regex.Match(text, @"stage(?<stageID>\d+)\s+level(?<levelID>\d+)");
        // if (match.Success)
        // {
        //     int stageID = int.Parse(match.Groups["stageID"].Value);
        //     int levelID = int.Parse(match.Groups["levelID"].Value);

        GameData.currentStage = stageID;
        GameData.currentLevel = levelID;


        foreach (var level in GameData.levelDB.levels)
        {
            Debug.Log("level");
            if (level.stageID == GameData.currentStage && level.levelID == GameData.currentLevel)
            {
                Debug.Log("load gamedata cur level.");
                GameData.tileIndices = level.tileIndices;
                GameData.shapeDataIndices = level.shapeDataIndices;
                GameData.solutions = level.solutions;
                GameData.shapeColor = level.shapeColor;
                break;
            }
        }
        SceneManager.LoadScene("Play");

    }
}
