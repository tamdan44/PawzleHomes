using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtun : MonoBehaviour, IPointerClickHandler
{
    public List<Starr> starList = new();
    public GameObject inactiveText;
    public GameObject activeText;

    public bool levelCleared;
    public bool fullCleared;    //no hints

    //returns the number of active stars (to activate new levels?)
    private void Awake()
    {
        levelCleared = false;
        fullCleared = false;
        activeText.SetActive(false);
    }
    public int IsStarActive()
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
        IsStarActive();
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
