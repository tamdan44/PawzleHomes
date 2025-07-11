using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelMenu : MonoBehaviour
{
    public List<LevelButton> levelButtons;
    public GameObject levelButtonPrefab;
    public Transform buttonContainer;


    //TODO: 
    //public Dictionary<(int, int), (bool, int)> completedLevelsDict; // save this so we can load it next session

    void Start()
    {
        LoadLevelButtons(0);

    }
    private void LoadLevelButtons(int stageID) // load images of levels for each stage
    {
                //TODO: levelButtons

    // number of levels for stageID, load images those levels
    }

    public void UpdateLevelButtons()
    {

        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        if (levelButtons == null)
        {
            levelButtons = new List<LevelButton>();
        }
        else
        {
            foreach (var btn in levelButtons)
            {
                Destroy(btn.gameObject);
            }
            levelButtons.Clear();
        }
        //for (int i = 0; i < 10; i++)  
        //{
        //    LevelButton levelButton = Instantiate(levelButtonPrefab, buttonContainer).GetComponent<LevelButton>();
        //    levelButton.name = $"Level {i + 1}";
        //    levelButton.levelNumber = i + 1;
        //    levelButtons.Add(levelButton);
        //}
    }

    public void Save(ref List<LevelData> dataList)
    {
        dataList.Clear();
        foreach (var btn in levelButtons)
        {
            var data = new LevelData();
            btn.Save(ref data);
            dataList.Add(data);
        }
    }
    
    public void Load(List<LevelData> dataList)
    {
        levelButtons.Clear();
        UpdateLevelButtons();
        foreach (var data in dataList)
        {
            LevelButton levelButton = Resources.Load<LevelButton>("Prefabs/LevelButton");

            if (levelButton != null)
            {
                LevelButton instantiatedButton = Instantiate(levelButton, transform);
                instantiatedButton.name = $"LevelButton {data.levelID}";
                instantiatedButton.Load(data);
                levelButtons.Add(instantiatedButton);


                bool isLevelUnlocked = bool.TryParse(data.solutions[2], out bool parsedUnlocked) ? parsedUnlocked : false;
                bool isLevelCompleted = bool.TryParse(data.solutions[0], out bool parsedCompleted) ? parsedCompleted : false;
                bool isFullCleared = bool.TryParse(data.solutions[1], out bool parsedFullCleared) ? parsedFullCleared : false;
                instantiatedButton.levelUnlocked = isLevelUnlocked;
                instantiatedButton.levelCleared = isLevelCompleted;
                instantiatedButton.fullCleared = isFullCleared;


                instantiatedButton.LoadNumberImage(data.levelID);
                instantiatedButton.ActivateStars();
            }
            else
            {
                Debug.LogError("Prefab 'LevelButton' không được tìm thấy trong thư mục Resources/Prefabs/");
            }
        }
    }


    public void LoadLevel(string text)
    {

        var match = Regex.Match(text, @"stage(?<stageID>\d+)\s+level(?<levelID>\d+)");
        if (match.Success)
        {
            int stageID = int.Parse(match.Groups["stageID"].Value);
            int levelID = int.Parse(match.Groups["levelID"].Value);

            GameData.currentStage = stageID;
            GameData.currentLevel = levelID;


            foreach (var level in GameData.levelDB.levels)
            {
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
        }
            SceneManager.LoadScene("Play");

    }

}
