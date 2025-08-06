using UnityEngine;
using UnityEngine.EventSystems;

public class StagePlayer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RevertFadeASyncLoading revertFadeASyncLoading;
    [SerializeField] private StageSwipe stageSwipe;
    [SerializeField] private GameObject content;
    private PanelAnimation[] panelAnimation;


    private void Awake()
    {
        if (GameData.stageUnlocked == null)
        {
            SaveSystem.LoadNewPlayer();
        }
        panelAnimation = new PanelAnimation[GameData.stageUnlocked.Length];

        panelAnimation = content.GetComponentsInChildren<PanelAnimation>();

        int i = 0;
        foreach (bool unlocked in GameData.stageUnlocked)
        {
            if (unlocked && i <= panelAnimation.Length)
            {
                if (GameData.stageTransition-1 == i)
                {
                    // mở khóa khi hoàn thành màn cuối của stage
                    UnlockStage(GameData.stageTransition);
                    GameData.stageTransition = 0;
                    SaveSystem.SavePlayer();
                }
                else
                panelAnimation[i].InitializeStageUnlocked();
            }
            i++;
        }
        
        // Debug.Log($"{panelAnimation.Length} + {GameData.stageUnlocked.Length} this is from stagePlayer ");
        // for (int j = 0; j < GameData.stageUnlocked.Length; j++)
        // {
        //     Debug.Log($"{panelAnimation[j].name} + {GameData.stageUnlocked[j]} this is from stagePlayer ");
        // }
    }

    public void TestUnlocking()
    {
        GameData.stageUnlocked[stageSwipe.currentStateIndex] = true;
        SaveSystem.SavePlayer();
        Debug.Log($"breaking {stageSwipe.currentStateIndex}");
    }

    public void OnPointerClick(PointerEventData eventData)  
    {
        if (GameData.stageUnlocked[stageSwipe.currentStateIndex] == true)
        {
            Debug.Log($"open stage {stageSwipe.currentStateIndex}");
            GameData.currentStage = stageSwipe.currentStateIndex;
            revertFadeASyncLoading.PlayRevertAndLoadDefault();
            // SceneManager.LoadScene("LevelScreen");
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
