using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StagePlayer : MonoBehaviour, IPointerClickHandler
{
    public SwipeMenu stageData;
    public GameObject content;
    private PanelAnimation[] panelAnimation;

    private void Start()
    {
        panelAnimation = new PanelAnimation[stageData.stageUnlocked.Length];
        panelAnimation = content.GetComponentsInChildren<PanelAnimation>();
    }

    public void TestUnlocking()
    {
        stageData.stageUnlocked[stageData.currentIndex] = true;
        Debug.Log("breaking");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(stageData.currentIndex);
        if (stageData.stageUnlocked[stageData.currentIndex] == true)
        {
            Debug.Log("Unlocked");
            Debug.Log(panelAnimation[stageData.currentIndex]);
            panelAnimation[stageData.currentIndex].Running();
            SceneManager.LoadScene("ChapterScreen");
        }
        else Debug.Log("Still locked");
    }

}
