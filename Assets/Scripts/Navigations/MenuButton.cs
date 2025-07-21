using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Mono.Cecil;
using System;

public class MenuButtons : MonoBehaviour
{
    void Awake()
    {
        if (!Application.isEditor)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }
    void OnEnable()
    {
        GameEvents.OpenLevel += OpenLevel;
    }
    void OnDisable()
    {
        GameEvents.OpenLevel -= OpenLevel;
    }
    public void LoadScreen(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadNextLevel()
    {
            Debug.Log($"GameData.currentLevel {GameData.currentLevel}");
        GameData.currentLevel += 1;
        GameEvents.OpenLevel(GameData.currentStage, GameData.currentLevel);
    }

    bool IsDuplicatedLevel(int stageID, int levelID) =>
        GameData.levelDB.levels.Any(l => l.stageID == stageID && l.levelID == levelID);

    public void OpenLevel(int stageID, int levelID) 
    {
        GameData.currentStage = stageID;
        GameData.currentLevel = levelID;
        Debug.Log($"OpenLevel {GameData.currentLevel}");


        var level = GameData.levelDB.levels
            .FirstOrDefault(l => l.stageID == stageID && l.levelID == levelID);

        if (level != null)
        {
            Debug.Log($"load gamedata OpenLevel. {GameData.currentLevel}");
            GameData.tileIndices = level.tileIndices;
            GameData.shapeDataIndices = level.shapeDataIndices;
            GameData.solutions = level.solutions;
            GameData.shapeColor = level.shapeColor;
        }
        if (SceneManager.GetActiveScene().name == "Puzzle")
        {
            SceneManager.LoadScene("Puzzle");
        }
        else
        {
            SceneManager.LoadScene("Play");
        }

        

    }
}
