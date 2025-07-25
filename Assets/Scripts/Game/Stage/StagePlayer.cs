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
            if (unlocked && i <= panelAnimation.Length)
            {
                panelAnimation[i].InitializeStageUnlocked();
                Debug.Log(i);
            }
            i++;
        }
        for (int j = 0; j < GameData.stageUnlocked.Length; j++)
        {
            Debug.Log($"{panelAnimation[j].name} + {GameData.stageUnlocked[j]} this is from stagePlayer ");
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
            Debug.Log($"open stage {stageSwipe.currentStateIndex}");
            GameData.currentStage = stageSwipe.currentStateIndex;
            SceneManager.LoadScene("LevelScreen");
        }
        else Debug.Log("Still locked");
    }

    void UnlockStage(int stageID)
    {
        Debug.Log("Unlocked");
        Debug.Log(panelAnimation[stageID-1]);
        panelAnimation[stageID-1].Running();

    }

}
