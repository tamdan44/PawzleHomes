using Assets.Scripts.ChapterScreen.Data;
using Assets.Scripts.SaveLoad;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class LevelMenu : MonoBehaviour
{
    #region Variables
    [Header("Level Buttons")]
    [HideInInspector]
    public List<LevelButton> levelButtons = new List<LevelButton>();
    [Tooltip("Maximum number of buttons allowed")]

    [SerializeField] private int createLevelButtonCount = 12;
    [SerializeField] private int stageNumber = 1;
    [SerializeField] private int chapterNumber = 1;


    [Header("UI Prefabs")]
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private Transform buttonContainer;

    private ChapterStageLevelManager chapterManager;
    #endregion

    // LevelMenu.cs
    void Start()
    {
        // 1. Validate prefab + container
        if (!CheckButtonPrefabs() || !CheckButtonContainer())
            return;
        levelButtonPrefab.InitializeUI();
        // 2. KHỞI TẠO NGAY chapterManager
        chapterManager = new ChapterStageLevelManager(SaveSystem.GetSaveData());

        // 3. Kiểm tra file
        if (SaveSystem.CheckFileExists())
        {
            // 3a. Nếu có: load data rồi build UI
            SaveSystem.Load();
            chapterManager.Load(
                levelButtonPrefab,
                buttonContainer,
                SaveSystem.GetSaveData().chapters,
                levelButtons
            );
        }
        else
        {
            // 3b. Nếu chưa: khởi tạo mặc định (giờ chapterManager đã có)
            InitialButtonLevel();
        }


    }
    private void LoadLevelButtons(int stageID) // load images of levels for each stage
    {
                //TODO: levelButtons

    // number of levels for stageID, load images those levels
    }

    public void CompleteLevel(int levelNumber)
    {

        LevelButton levelButton = levelButtons.Find(btn => btn.levelNumber == levelNumber);
        if (levelButton != null)
        {
            levelButton.levelCleared = true;

            chapterManager.UpdateLevel(chapterNumber, stageNumber, levelNumber, levelButton.fullCleared ? 2 : levelButton.levelCleared ? 1 : levelButton.levelUnlocked ? 0 : -1, levelButton.fullCleared ? 100 : levelButton.levelCleared ? 70 : 0, "Level description");
            SaveSystem.Save();
        }
    }

    #region Validation
    private void OnValidate()
    {
        if (stageNumber < 1)
        {
            Debug.LogError("Stage ID phải nằm trong khoảng từ 1 đến 50");
            stageNumber = Mathf.Clamp(stageNumber, 1, 50);
        }

        if (createLevelButtonCount < 0 || createLevelButtonCount > 12)
        {
            Debug.LogError("Số lượng nút phải nằm trong khoảng từ 0 đến 12.");
            createLevelButtonCount = Mathf.Clamp(createLevelButtonCount, 0, 12);
        }

        if (levelButtons.Count > createLevelButtonCount)
        {
            levelButtons.RemoveRange(createLevelButtonCount, levelButtons.Count - createLevelButtonCount);
            Debug.LogWarning("Số lượng buttons vượt quá giới hạn. Đã xóa bớt các button thừa.");
        }
    }

    private bool CheckButtonPrefabs()
    {
        if (levelButtonPrefab == null)
        {
            Debug.LogError("LevelButton Prefab chưa được gắn vào trong Inspector. Tự động tìm LevelButton Prefab ở thư mục: Asset/Resourse/Prefabs/LevelButton");
            levelButtonPrefab = Resources.Load<LevelButton>("Prefabs/LevelButton");
            if (levelButtonPrefab == null)
            {
                Debug.LogError("Không tìm thấy LevelButton Prefab trong thư mục Resources. Vui lòng kiểm tra lại đường dẫn hoặc gắn Prefab vào trong Inspector.");
                return false;
            }
            else
            {
                Debug.Log("Tự động tìm LevelButton Prefab thành công.");
                return true;
            }
        }
        return true;
    }

    private bool CheckButtonContainer()
    {
        if (buttonContainer == null)
        {
            var levelMenu = GameObject.Find("LevelMenu");
            if (levelMenu == null)
                levelMenu = GameObject.Find("Levels");
            if (levelMenu != null)
            {
                buttonContainer = levelMenu?.transform;
                if (buttonContainer == null)
                {
                    Debug.LogError("Không tìm thấy ButtonContainer trong LevelMenu. Vui lòng kiểm tra cấu trúc GameObject.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy GameObject LevelMenu. Vui lòng kiểm tra cấu trúc GameObject.");
                return false;
            }
            Debug.Log("Tự động tìm ButtonContainer trong LevelMenu thành công");
        }
        return true;
    }
    #endregion

    #region Button Management
    private void InitialButtonLevel()
    {
        Debug.Log("Tiến hành khởi tạo dữ liệu tự động");

        // Nếu container chưa gán (Inspector hoặc Find), thử gọi lại
        if (buttonContainer == null && !CheckButtonContainer())
        {
            Debug.LogError("Không tìm được buttonContainer, hủy khởi tạo.");
            return;
        }

        // Xóa các nút cũ (nếu có)
        DeleteAllLevelButtons();

        // Tạo Chapter – Stage mặc định
        chapterManager.CreateNewChapter(chapterNumber, $"Chapter {chapterNumber}");
        chapterManager.CreateNewStage(chapterNumber, stageNumber, $"Stage {stageNumber}");

        // Tạo các LevelButton mới và lưu vào SaveData
        CreateLevelButtons(createLevelButtonCount);

        SaveSystem.Save();
    }


    private void CreateLevelButtons(int numberOfButtons)
    {
        for (int i = 0; i < numberOfButtons; i++)
        {
            LevelButton newButton = Instantiate(levelButtonPrefab, buttonContainer);
            newButton.name = $"LevelButton {i + 1}";
            newButton.enabled = true;
            newButton.levelNumber = i + 1;
            newButton.levelCleared = false;
            newButton.fullCleared = false;
            newButton.levelUnlocked = i == 0; // Chỉ nút đầu tiên được mở khóa

            levelButtons.Add(newButton);
            newButton.InitializeUI();

            newButton.gameObject.SetActive(true);

            // Gọi phương thức lưu các level trong ChapterStageLevelManager
            chapterManager.AddLevelToStage(chapterNumber, stageNumber, newButton.levelUnlocked ? 0 : -1, 0, "Description", "path_to_image");

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
