using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StagePlayer : MonoBehaviour, IPointerClickHandler
{
    public StageSwipe stageSwipe;
    public GameObject content;
    private PanelAnimation[] panelAnimation;


    private void Awake()
    {
        SaveSystem.LoadPlayer();

        panelAnimation = new PanelAnimation[GameData.stageUnlocked.Length];
        panelAnimation = content.GetComponentsInChildren<PanelAnimation>();

        int i = 0;
        foreach (bool unlocked in GameData.stageUnlocked)
        {
            if (unlocked == true && i <= panelAnimation.Length)
            {
                //panelAnimation[i].InitializeStageUnlocked();
                Debug.Log(GameData.stageUnlocked[i]);
            }
            i++;
        }
    }

    public void TestUnlocking()
    {
        GameData.stageUnlocked[stageSwipe.currentStateIndex] = true;
        SaveSystem.SavePlayer();
        Debug.Log($"breaking {stageSwipe.currentStateIndex}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(stageSwipe.currentStateIndex);
        if (GameData.stageUnlocked[stageSwipe.currentStateIndex] == true)
        {
            if (stageSwipe.currentStateIndex > 0)
            {
                Debug.Log("Unlocked");
                Debug.Log(panelAnimation[stageSwipe.currentStateIndex]);
                panelAnimation[stageSwipe.currentStateIndex].Running();
            }
            if (stageSwipe.currentStateIndex == 0)
            {
                GameData.currentStage = stageSwipe.currentStateIndex; 
                SceneManager.LoadScene("LevelScreen");
            }
        }
        else Debug.Log("Still locked");
    }

}
