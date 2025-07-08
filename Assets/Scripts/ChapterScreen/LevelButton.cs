using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    public List<Starr> starList = new();
    public GameObject inactiveText; // to be removed
    public GameObject activeText; // to be removed
    public List<Image> inactiveNumbers;//TODO: make a function to load the right num
    public List<Image> activeNumbers;//TODO: make a function to load the right num
    public int levelNumber; //TODO: make a function to load the right num

    public bool levelCleared; // 1*
    public bool fullCleared;    //no hints 2*
    public bool levelUnlocked; //true: player click this button -> load level
    private int thisStage, thisLevel;

    //returns the number of active stars (to activate new levels?)
    private void Awake()
    {
        // TODO: load player data of thislevel, thisstage
        levelUnlocked = false;
        levelCleared = false;
        fullCleared = false;
        activeText.SetActive(false);
        LoadNumberImage(levelNumber);
    }
    void LoadNumberImage(int number) //TODO: make a function to load the right num
    {
        
    }
    // TODO: swipe levels and dot

    public int StarCount()
    {
        int starCount = 0;
        foreach (var item in starList)
        {
            if (starList[starCount].IsActive())
            {
                starCount++;
            }
            else break;
        }
        Debug.Log(starCount);
        return starCount;
    }
    //to test the code
    public void OnPointerClick(PointerEventData eventData)
    {
        StarCount();
        ActivateStars();
    }

    public void ActivateStars()
    {
        if (levelCleared)
        {
            starList[0].SetStarActive();
            activeText.SetActive(true);
            if (fullCleared)
            {
                foreach (var item in starList)
                {
                    item.SetStarActive();
                }
            }
        }
    }

    void UnlockNextLevel()
    {
        int nextLevel = levelNumber + 1;
        GameObject nextLevelObject = GameObject.Find($"Level {nextLevel}");
        if (nextLevelObject != null)
        {
            LevelButton nextLevelButton = nextLevelObject.GetComponent<LevelButton>();
            if (nextLevelButton != null)
            {
                nextLevelButton.levelUnlocked = true;
                if (!fullCleared)
                    Debug.Log($"Next level unlock: level {nextLevel}");
                else
                {
                    Debug.Log("Congratulations you have completed all the challenges in this level");
                }
                nextLevelButton.UpdateLevelUI(nextLevel, true, false); // Update the UI for the next level
            }
        }
    }


    public void UpdateLevelUI(int levelNumber, bool isLevelUnlocked, bool isLevelCompleted)
    {
        GameObject level = GameObject.Find($"Level {levelNumber}");

        level.SetActive(isLevelUnlocked);

        GameObject levelTextInactive = level.transform.Find("LevelText/Level Inactive").gameObject;
        GameObject levelTextActive = level.transform.Find("LevelText/Level Active").gameObject;
        levelTextInactive.GetComponent<Image>().color = new Color(0.7f, 0f, 0.5f, 1f);
        levelTextInactive.SetActive(!isLevelCompleted);
        levelTextActive.SetActive(isLevelCompleted);
    }

    void SetStarsForLevel(int levelNumber, int starsAchieved)
    {
        GameObject level = GameObject.Find($"Level {levelNumber}");
        var starContainer = level.transform.Find("Stars");

        for (int i = 0; i < starContainer.childCount; i++)
        {
            Transform star = starContainer.GetChild(i);
            star.gameObject.SetActive(i < starsAchieved);
        }
    }
}
