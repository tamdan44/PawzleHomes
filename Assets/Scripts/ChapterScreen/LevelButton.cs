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

}
