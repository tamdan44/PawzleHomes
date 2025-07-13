using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StagePlayer : MonoBehaviour, IPointerClickHandler
{
    public SwipeMenu stagePlayer;
    public GameObject content;
    private PanelAnimation[] panelAnimation;

    private void Start()
    {
        panelAnimation = new PanelAnimation[stagePlayer.stageUnlocked.Length];
        panelAnimation = content.GetComponentsInChildren<PanelAnimation>();
    }

    public void TestUnlocking()
    {
        stagePlayer.stageUnlocked[stagePlayer.currentStateIndex] = true;
        Debug.Log("breaking");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(stagePlayer.currentStateIndex);
        if (stagePlayer.stageUnlocked[stagePlayer.currentStateIndex] == true)
        {
            Debug.Log("Unlocked");
            Debug.Log(panelAnimation[stagePlayer.currentStateIndex]);
            panelAnimation[stagePlayer.currentStateIndex].Running();
            GameData.currentStage = stagePlayer.currentStateIndex; 
            SceneManager.LoadScene("ChapterScreen");
        }
        else Debug.Log("Still locked");
    }

}
