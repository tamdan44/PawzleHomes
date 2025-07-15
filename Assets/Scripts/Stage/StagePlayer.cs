using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StagePlayer : MonoBehaviour, IPointerClickHandler
{
    public StageSwipe stagePlayer;
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
            if (unlocked)
            {
                panelAnimation[i].InitializeStageUnlocked();
            }
            i++;
        }
    }

    public void TestUnlocking()
    {
        GameData.stageUnlocked[stagePlayer.currentStateIndex] = true;
        SaveSystem.SavePlayer();
        Debug.Log("breaking");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(stagePlayer.currentStateIndex);
        if (GameData.stageUnlocked[stagePlayer.currentStateIndex] == true)
        {
            if (stagePlayer.currentStateIndex > 0)
            {
                Debug.Log("Unlocked");
                Debug.Log(panelAnimation[stagePlayer.currentStateIndex]);
                panelAnimation[stagePlayer.currentStateIndex].Running();
            }
            if (stagePlayer.currentStateIndex == 0)
            {
                GameData.currentStage = stagePlayer.currentStateIndex; 
                SceneManager.LoadScene("LevelScreen");
            }
        }
        else Debug.Log("Still locked");
    }

}
