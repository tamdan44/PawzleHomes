// using Assets.Scripts.ChapterScreen.Data;
// using Assets.Scripts.SaveLoad;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuNew : MonoBehaviour
{
    #region Variables
    [Header("Level Buttons")]
    [HideInInspector]
    public List<LevelButtonNew> levelButtons = new();

    
    [Tooltip("Maximum number of buttons allowed")]
    [SerializeField] private int buttonsPerPage = 12;
    private int stageNumber;

    [Header("UI Prefabs")]
    [SerializeField] private LevelButtonNew levelButtonPrefab;
    [SerializeField] private GameObject levelContainer;
    [SerializeField] private Transform content;

    // private ChapterStageLevelManager chapterManager;
    #endregion



    // LevelMenu.cs
    void Awake()
    {
        stageNumber = GameData.currentStage == 0 ? 1 : GameData.currentStage;
        Debug.Log($"{stageNumber}");

        SaveSystem.LoadPlayer();

        levelButtonPrefab.InitializeUI();
        CreateLevelButtons(GameData.stageLevelDict[stageNumber], buttonsPerPage);
    }


    #region Button Management
    // private void InitialButtonLevel()
    // {

    //     // Xóa các nút cũ (nếu có)
    //     DeleteAllLevelButtons();

    //     // Tạo Chapter – Stage mặc định
    //     // chapterManager.CreateNewChapter(chapterNumber, $"Chapter {chapterNumber}");
    //     // chapterManager.CreateNewStage(chapterNumber, stageNumber, $"Stage {stageNumber}");

    //     // Tạo các LevelButton mới và lưu vào SaveData
    //     CreateLevelButtons(createLevelButtonCount);

    //     // SaveSystem.Save();
    // }

    private void CreateLevelButtons(int numberOfButtons, int pageButtons)
    {
        if (pageButtons > numberOfButtons) pageButtons = numberOfButtons;
        int maximumButtons = pageButtons;

        int buttonIndex = 1;
        int pageCount = Mathf.CeilToInt(numberOfButtons / pageButtons) + 1;

        GameObject[] levels = new GameObject[pageCount];
        for (int j = 0; j < pageCount; j++)
        {
            levels[j] = Instantiate(levelContainer, content);

            for (int i = 0; i < maximumButtons; i++)
            {
                LevelButtonNew newButton = Instantiate(levelButtonPrefab, levels[j].transform);
                newButton.enabled = true;
                newButton.levelNumber = buttonIndex;
                newButton.stageNumber = stageNumber;
                newButton.levelCleared = false;
                newButton.fullCleared = false;

                levelButtons.Add(newButton);
                newButton.InitializeUI();

                newButton.gameObject.SetActive(true);
                buttonIndex++;
                // Gọi phương thức lưu các level trong ChapterStageLevelManager
                // chapterManager.AddLevelToStage(chapterNumber, stageNumber, newButton.levelUnlocked ? 0 : -1, 0, "Description", "path_to_image");

                Debug.Log($"Đã tạo LevelButton {buttonIndex + 1} cho  Stage {stageNumber}");
            }
            numberOfButtons -= pageButtons;
            if (numberOfButtons >= pageButtons) maximumButtons = pageButtons;
            else maximumButtons = numberOfButtons;
        }
        if (levels[^1].transform.childCount == 0) Destroy(levels[^1]);
        levelButtons[0].levelUnlocked = 0 == 0; // Chỉ nút đầu tiên được mở khóa
    }

    /*public void DeleteAllLevelButtons()
    {
        Debug.LogWarning($"Xóa tất cả obj con trong buttonContainer");
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        levelButtons.Clear();
    }*/
    #endregion
}
