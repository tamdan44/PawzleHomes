// using Assets.Scripts.SaveLoad;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtonNew : MonoBehaviour, IPointerClickHandler
{
    [Header("Stars and Texts")]
    public List<Starr> starList = new List<Starr>();
    public TMP_Text levelText; 

    [Header("Number Images")]
    public List<Image> inactiveNumbers; // Visible when not cleared
    public List<Image> activeNumbers;   // Visible when cleared

    [Header("Level Info")]
    public int levelNumber;
    public bool levelCleared;
    public bool fullCleared;
    public bool levelUnlocked;
    public int stageNumber;

    // Registry for unlocking logic
    private static List<LevelButtonNew> allLevelButtons = new();

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



    public void InitializeUI()
    {
        // Colors
        Color inactiveColor = levelUnlocked && !levelCleared
            ? new Color(0.7f, 0f, 0.5f, 1f)
            : new Color(1f, 1f, 1f, 0.6f);
        Color activeColor = new(0f, 1f, 0.5f, 1f);

        //levelText.sprite = levelSprite;
        levelText.text = levelNumber.ToString();

        int status = -1;
        GameData.playerLevelData.TryGetValue((stageNumber, levelNumber), out status);

        if (status == -1) levelUnlocked = false;
        if (status >= 0) levelUnlocked = true;
        // 
        
        if (!levelUnlocked)
        {
            levelText.color = inactiveColor;
        }
        else
        {
            levelText.color = activeColor;
        }

        ActivateStars();
        
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!levelUnlocked)
        {
            Debug.Log($"Level {levelNumber} is locked.");
            return;
        }
        else
        {
            Debug.Log($"level {levelNumber} is unlocked");
        }

        // InitializeUI();
        // SaveGameState();

        // GameEvents.OpenLevel(stageNumber, levelNumber);
    }

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

    /// <summary>
    /// Activate star visuals and unlock next level
    /// </summary>
    private void ActivateStars(bool confirmSave = true)
    {
        if (!levelCleared)
            return;

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

    private void SaveGameState()
    {
        // Assuming a central manager handles saving all levels
        // SaveSystem.Save();
    }

    #region Save and Load for manager
    public void Save(ref StageLevelData data)
    {
        data.levelNumber = levelNumber;
        data.status = fullCleared ? 2 : levelCleared ? 1 : levelUnlocked ? 0 : -1;
        data.score = StarCount();
    }

    public void Load(StageLevelData data)
    {
        levelNumber = data.levelNumber;
        levelCleared = (data.status == 1);
        fullCleared = (data.status == 2);
        levelUnlocked = (data.status != -1);
        InitializeUI();
    }
    #endregion
}
