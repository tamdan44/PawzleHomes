using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public List<LevelButton> levelButtons;
    public GameObject levelButtonPrefab;
    public Transform buttonContainer;

    private void Awake()
    {

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
}
