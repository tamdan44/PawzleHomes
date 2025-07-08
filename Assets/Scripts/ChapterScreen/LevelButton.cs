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
        if (levelNumber == 1) // level 1 is always active except the final level previous Stage is not unlocked
        {
            Debug.Log("Level 1 is always active, so it is unlocked by default.");
            levelUnlocked = true;
        }
        else
            levelUnlocked = false;
        levelCleared = false;
        fullCleared = false;
        activeText.SetActive(false);
        LoadNumberImage(levelNumber);
    }
    void LoadNumberImage(int number) //TODO: make a function to load the right num
    {

        string spriteName = $"{number}_brown";


        Sprite levelSprite = Resources.Load<Sprite>($"Sprites/LevelScreen/{spriteName}");

        if (levelSprite != null)
        {

            // Apply the sprite to the relevant UI Image component
            foreach (var image in inactiveNumbers)
            {
                image.sprite = levelSprite;
                if (number > 9)
                    image.rectTransform.sizeDelta = new Vector2(110, 110);
                if (levelUnlocked && !levelCleared && !fullCleared)
                    image.color = new Color(0.7f, 0f, 0.5f, 1f);
                else
                    image.color = new Color(1f, 1f, 1f, 0.6f);
            }
            foreach (var image in activeNumbers)
            {
                image.sprite = levelSprite;
                if (number > 9)
                    image.rectTransform.sizeDelta = new Vector2(110, 110);
                image.color = new Color(0f, 1f, 0.5f, 1f);
            }

        }
        else
        {
            Debug.LogError($"Sprite '{spriteName}' not found.");
        }


    }
    // TODO: swipe levels and dot

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
        if (starCount == 0)
        {
            Debug.Log("Level" + levelNumber.ToString() + " FAIL");
        }
        else if (starCount == 1)
        {
            Debug.Log("Level" + levelNumber.ToString() + " is cleared. You achieve 1 star");
        }
        else if (starCount == 2)
        {
            Debug.Log("Level" + levelNumber.ToString() + " is fully cleared. You achieve 2 star");
        }

        return starCount;
    }
    //to test the code
    public void OnPointerClick(PointerEventData eventData)
    {

        if (!levelUnlocked)
        {
            Debug.Log($"Level" + levelNumber.ToString() + "is locked. To unlock you need to achieve atleast 1 start at level " + (levelNumber - 1).ToString());
            return; // Exit if the level is not unlocked
        }

        Debug.Log($"Enter level" + levelNumber.ToString());

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
                foreach (var star in starList)
                {
                    star.SetStarActive();
                }
            }
            int starCount = StarCount();
            if (starCount > 0)
            {
                UnlockNextLevel(); // IF the level is cleared, unlock the next level

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
