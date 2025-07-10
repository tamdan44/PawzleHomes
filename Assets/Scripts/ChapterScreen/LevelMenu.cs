using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class LevelMenu : MonoBehaviour
{
    public List<LevelButton> levelButtons;
    private LevelDatabase levelDB;
    private const string filePath = "Assets/Resources/levels.json";

    //TODO: 
    //public Dictionary<(int, int), (bool, int)> completedLevelsDict; // save this so we can load it next session


    void Start()
    {


        LoadLevelButtons(0);

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
    private void LoadLevelButtons(int stageID) // load images of levels for each stage
    {
                //TODO: levelButtons

    // number of levels for stageID, load images those levels
    }



        public void LoadLevel() 
        {
            // OpenLevel(,);
        }

    public void OpenLevel(string text) // fix this to make it load 
    {

        var match = Regex.Match(text, @"stage(?<stageID>\d+)\s+level(?<levelID>\d+)");
        if (match.Success)
        {
            int stageID = int.Parse(match.Groups["stageID"].Value);
            int levelID = int.Parse(match.Groups["levelID"].Value);

            GameData.currentStage = stageID;
            GameData.currentLevel = levelID;

            SceneManager.LoadScene("Play 1");
        }
        else
        {
            Debug.Log("Input string format is invalid.");
        }

        foreach (var level in levelDB.levels)
        {
            if (level.stageID == GameData.currentStage && level.levelID == GameData.currentLevel)
            {
                Debug.Log("load gamedata cur level.");
                GameData.tileIndices = level.tileIndices;
                GameData.shapeDataIndices = level.shapeDataIndices;
                GameData.solutions = level.solutions;
                GameData.shapeColor = level.shapeColor;
            }
        }


    }
}
