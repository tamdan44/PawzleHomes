using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{

    public List<Starr> starList = new List<Starr>();
    public GameObject inactiveText;
    public GameObject activeText;
    public List<Image> inactiveNumbers;
    public List<Image> activeNumbers;
    public int levelNumber;
    public bool levelCleared;
    public bool fullCleared;
    public bool levelUnlocked;
    public int stageId = 0;

    private void Awake()
    {
        InitialLoad();
    }
    void InitialLoad()
    {
        if (levelNumber == 1)
        {
            levelUnlocked = true;
        }
        else
        {
            levelUnlocked = false;
        }
        levelCleared = false;
        fullCleared = false;
        activeText.SetActive(false);
        LoadNumberImage(levelNumber);
    }
    public void LoadNumberImage(int number)
    {
        string spriteName = $"{number}_brown";
        Sprite levelSprite = Resources.Load<Sprite>($"Sprites/LevelScreen/{spriteName}");

        if (levelSprite != null)
        {
            foreach (var image in inactiveNumbers)
            {
                image.sprite = levelSprite;
                image.color = levelUnlocked && !levelCleared && !fullCleared ? new Color(0.7f, 0f, 0.5f, 1f) : new Color(1f, 1f, 1f, 0.6f);
                if (number > 9)
                    image.rectTransform.sizeDelta = new Vector2(110f, 110f);
            }
            foreach (var image in activeNumbers)
            {
                image.sprite = levelSprite;
                image.color = new Color(0f, 1f, 0.5f, 1f);
                if (number > 9)
                    image.rectTransform.sizeDelta = new Vector2(110f, 110f);
            }
        }
        else
        {
            Debug.LogError($"Sprite '{spriteName}' not found.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!levelUnlocked)
        {
            Debug.Log($"Level {levelNumber} is locked.");
            return;
        }
        ActivateStars();

        Debug.Log($"Enter level {levelNumber}");
        GameEvents.OpenLevel(stageId, levelNumber);
        // SceneManager.LoadScene("Play");
    }

    public int StarCount()
    {
        int starCount = 0;
        foreach (var star in starList)
        {
            if (star.IsActive())
            {
                starCount++;
            }
        }

        return starCount;
    }

    public void ActivateStars()
    {
        if (levelCleared)
        {
            starList[0].SetStarActive();
            activeText.SetActive(true);
            if (fullCleared)
            {
                foreach (var star in starList)
                {
                    star.SetStarActive();
                }
            }
            int starCount = StarCount();
            if (starCount > 0)
            {
                UnlockNextLevel();
            }
        }
    }

    void UnlockNextLevel()
    {
        int nextLevel = levelNumber + 1;
        GameObject nextLevelObject = GameObject.Find($"LevelButton {nextLevel}");
        if (nextLevelObject != null)
        {
            LevelButton nextLevelButton = nextLevelObject.GetComponent<LevelButton>();
            if (nextLevelButton != null)
            {
                nextLevelButton.levelUnlocked = true;
                nextLevelButton.UpdateLevelUI(nextLevel, true, false);
                nextLevelButton.LoadNumberImage(nextLevel);
            }
        }
    }

    public void UpdateLevelUI(int levelNumber, bool isLevelUnlocked, bool isLevelCompleted)
    {
        GameObject level = GameObject.Find($"LevelButton {levelNumber}");
        level.SetActive(isLevelUnlocked);

    }

    #region Save and Load

    public void Save(ref LevelData data)
    {
        data.levelID = levelNumber;
        data.stageID = stageId;
        data.clears = new List<string>
        {
            levelCleared.ToString(),
            fullCleared.ToString(),
            levelUnlocked.ToString()
        };
        // data.tileIndices = new List<Vector3Int>();
        // data.shapeDataIndices = new List<int>();
    }

    public void Load(LevelData data)
    {
        levelNumber = data.levelID;
        stageId = data.stageID;
        levelCleared = bool.Parse(data.clears[0]);
        fullCleared = bool.Parse(data.clears[1]);
        levelUnlocked = bool.Parse(data.clears[2]);
    }

    #endregion  
}
