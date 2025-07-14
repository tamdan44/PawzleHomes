// using Assets.Scripts.ChapterScreen.Data;
// using Assets.Scripts.SaveLoad;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelMenu : MonoBehaviour
{
    #region Variables
    [Header("Level Buttons")]
    [HideInInspector]
    public List<LevelButtonNew> levelButtons = new List<LevelButtonNew>();
    [Tooltip("Maximum number of buttons allowed")]

    [SerializeField] private int createLevelButtonCount = 10;
    private int stageNumber;
    [SerializeField] private int chapterNumber = 1;


    [Header("UI Prefabs")]
    [SerializeField] private LevelButtonNew levelButtonPrefab;
    [SerializeField] private Transform buttonContainer;

    // private ChapterStageLevelManager chapterManager;
    #endregion



    // LevelMenu.cs
    void Start()
    {
        stageNumber = GameData.currentStage == 0 ? 1 : GameData.currentStage;
        Debug.Log($"{stageNumber}");

        levelButtonPrefab.InitializeUI();
        CreateLevelButtons(createLevelButtonCount);
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


    private void CreateLevelButtons(int numberOfButtons)
    {
        for (int i = 0; i < numberOfButtons; i++)
        {
            LevelButtonNew newButton = Instantiate(levelButtonPrefab, buttonContainer);
            newButton.enabled = true;
            newButton.levelNumber = i + 1;
            newButton.levelCleared = false;
            newButton.fullCleared = false;
            newButton.levelUnlocked = i == 0; // Chỉ nút đầu tiên được mở khóa

            levelButtons.Add(newButton);
            newButton.InitializeUI();

            newButton.gameObject.SetActive(true);

            // Gọi phương thức lưu các level trong ChapterStageLevelManager
            // chapterManager.AddLevelToStage(chapterNumber, stageNumber, newButton.levelUnlocked ? 0 : -1, 0, "Description", "path_to_image");

            Debug.Log($"Đã tạo LevelButton {i + 1} cho Chapter {chapterNumber}, Stage {stageNumber}");
        }
    }

    public void DeleteAllLevelButtons()
    {
        Debug.LogWarning($"Xóa tất cả obj con trong buttonContainer");
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        levelButtons.Clear();
    }
    #endregion
}
