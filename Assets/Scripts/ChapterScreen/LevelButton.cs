using Assets.Scripts.SaveLoad;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Stars and Texts")]
    public List<Starr> starList = new List<Starr>();
    public GameObject inactiveText; // Shown when locked
    public GameObject activeText;   // Shown when cleared

    [Header("Number Images")]
    public List<Image> inactiveNumbers; // Visible when not cleared
    public List<Image> activeNumbers;   // Visible when cleared

    [Header("Level Info")]
    public int levelNumber;
    public bool levelCleared = false;
    public bool fullCleared = false;
    public bool levelUnlocked = false;
    public int stageNumber = 1;
    public int chapterNumber = 1;

    [Header("Scene to Load")]
    [SerializeField] private string sceneToLoad;


    // Registry for unlocking logic
    private static List<LevelButton> allLevelButtons = new List<LevelButton>();

    private void Awake()
    {

        allLevelButtons.Add(this);
    }

    private void OnDestroy()
    {
        allLevelButtons.Remove(this);
    }

    private void Start()
    {
        InitializeUI();
    }


    #region UI Initialization and Interaction
    /// <summary>
    /// Set up visuals based on state (locked, cleared, etc.)
    /// </summary>
    public void InitializeUI()
    {
        // Set visibility based on unlocked state
        gameObject.SetActive(true);
        gameObject.name = $"LevelButton {stageNumber}-{levelNumber}";

        if (levelCleared || fullCleared)
        {
            activeText?.SetActive(true);
            inactiveText?.SetActive(false);
            // If cleared, show active numbers
            foreach (var img in activeNumbers)
            {
                img.enabled = true;
            }
        }
        else
        {
            activeText?.SetActive(false);
            inactiveText?.SetActive(true);
            // If not cleared, show inactive numbers
            foreach (var img in inactiveNumbers)
            {
                img.enabled = true;
            }
        }

        // Update number sprites and enable correct images
        UpdateNumberImages();

        // Reset all stars
        foreach (var star in starList)
            star.SetStarInactive();

        // Activate stars and next unlock if needed
        if (levelCleared || fullCleared)
            ActivateStars(confirmSave: false);
    }

    private void UpdateNumberImages()
    {
        string spriteName = $"{levelNumber}_brown";
        Sprite levelSprite = Resources.Load<Sprite>($"Sprites/LevelScreen/{spriteName}");
        if (levelSprite == null)
        {
            Debug.LogError($"LevelButton: Sprite '{spriteName}' not found.");
            return;
        }

        // Colors
        Color inactiveColor = levelUnlocked && !levelCleared
            ? new Color(0.7f, 0f, 0.5f, 1f)
            : new Color(1f, 1f, 1f, 0.6f);
        Color activeColor = new Color(0f, 1f, 0.5f, 1f);

        // Inactive images visible when not cleared
        foreach (var img in inactiveNumbers)
        {
            img.sprite = levelSprite;
            img.color = inactiveColor;
            img.enabled = !levelCleared;
            if (levelNumber > 9)
                img.rectTransform.sizeDelta = new Vector2(110f, 110f);
        }
        // Active images visible when cleared
        foreach (var img in activeNumbers)
        {
            img.sprite = levelSprite;
            img.color = activeColor;
            img.enabled = levelCleared;
            if (levelNumber > 9)
                img.rectTransform.sizeDelta = new Vector2(110f, 110f);
        }
    }

    /// <summary>
    /// Activate star visuals and unlock next level
    /// </summary>
    private void ActivateStars(bool confirmSave = true)
    {

        if (!levelUnlocked)
        {
            Debug.LogWarning($"Level {levelNumber} is locked. Cannot activate stars.");
            foreach (var star in starList)
                star.SetStarInactive();
            return;
        }


        if (levelCleared != fullCleared)
        {
            if (fullCleared)
            {
                Debug.LogWarning($"Level {levelNumber} is not cleared but fullCleared is true. Set true all");
                fullCleared = true;
                levelCleared = true;
                foreach (var star in starList)
                    star.SetStarActive();
                return;
            }
        }
        else
        if (!levelCleared)
        {
            Debug.LogWarning($"Level {levelNumber} is not cleared. Cannot activate stars.");
            foreach (var star in starList)
                star.SetStarInactive();
            return;

        }
        // Always activate the first star
        if (starList.Count > 0)
            starList[0].SetStarActive();

        if (fullCleared)
        {
            // Activate all stars
            foreach (var star in starList)
                star.SetStarActive();

        }

        // Unlock next level
        UnlockNextLevel();

        // Optionally save after unlocking
        if (confirmSave)
            SaveGameState();
    }


    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!levelUnlocked)
        {
            Debug.Log($"Level {levelNumber} is locked.");
            return;
        }
        ActivateStars();
        InitializeUI();
        SaveGameState();

        Debug.Log($"Opening Level {levelNumber} in Stage {stageNumber}");

        //GameEvents.OpenLevel(stageNumber, levelNumber); // On Dev

        if (SceneController.Instance != null)
        {
            // Dùng async + loading UI:
            SceneController.Instance.LoadSceneAsync(sceneToLoad);

            // Hoặc load nhanh:
            // SceneController.Instance.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("SceneController.Instance is null! Hãy chắc đã tạo GameObject có script SceneController.");
        }


    }

    #region Star and Level Management

    /// <summary>
    /// Count active stars
    /// </summary>
    public int StarCount()
    {
        int count = 0;
        foreach (var star in starList)
            if (star.IsActive()) count++;
        return count;
    }


    private void UnlockNextLevel()
    {
        int nextLevel = levelNumber + 1;
        var nextBtn = allLevelButtons.Find(b =>
            b.levelNumber == nextLevel &&
            b.stageNumber == stageNumber);

        if (nextBtn != null)
        {
            nextBtn.levelUnlocked = true;
            nextBtn.InitializeUI();
        }
    }
    #endregion


    #region Save and Load for manager
    private void SaveGameState()
    {
        // Assuming a central manager handles saving all levels
        SaveSystem.Save();
    }
    public void Save(ref StageLevelData data)
    {
        InitializeUI();
        data.levelNumber = levelNumber;
        data.status = fullCleared ? 2 : levelCleared ? 1 : levelUnlocked ? 0 : -1;
        data.score = StarCount();
    }

    public void Load(StageLevelData data)
    {
        levelNumber = data.levelNumber;
        levelCleared = (data.status >= 1);
        fullCleared = (data.status == 2);
        levelUnlocked = (data.status != -1);
        InitializeUI();
    }
    #endregion
}
