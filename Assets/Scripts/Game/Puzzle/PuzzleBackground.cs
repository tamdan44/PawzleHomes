using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBackground : MonoBehaviour
{
    private GameObject[] bgList;
    void Awake()
    {
        bgList = this.GetComponentsInChildren<Transform>(true).Where(t => t != this.transform).Select(t => t.gameObject).ToArray();
        Debug.Log($"bg count {bgList.Length}");
        for (int i = 0; i < GameData.currentLevel; i++)
        {
            bgList[0].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
