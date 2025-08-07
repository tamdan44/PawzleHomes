using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlayer : MonoBehaviour
{
    [SerializeField] private RevertFadeASyncLoading revertFadeASyncLoading;
    [SerializeField] private StageSwipe stageSwipe;
    [SerializeField] private GameObject content;
    [SerializeField] public PanelAnimation[] panelAnimations;
    
    [HideInInspector]
    public int panelTest;


    private void Awake()
    {
        if (GameData.stageUnlocked == null)
        {
            SaveSystem.LoadNewPlayer();
        }
        // panelAnimations = new PanelAnimation[GameData.stageUnlocked.Length];

        // panelAnimations = content.GetComponentsInChildren<PanelAnimation>();
        InitializeStages();
    }

    public void InitializeStages()
    {
        int i = 0;
        // panelTest = i+5;
        foreach (bool unlocked in GameData.stageUnlocked)
        {
            if (unlocked && i <= panelAnimations.Length)
            {
                if (GameData.stageTransition - 1 == i)
                {
                    // mở khóa khi hoàn thành màn cuối của stage
                    UnlockStage(GameData.stageTransition);
                    GameData.stageTransition = 0;
                    SaveSystem.SavePlayer();
                }
                else
                {
                    panelAnimations[i].InitializeStageUnlocked();
                    panelTest = i+5;
                }
            }
            i++;
        }

    }

    public void PlayButtonClicked()
    {
        if (GameData.stageUnlocked[stageSwipe.currentStateIndex] == true)
        {
            Debug.Log($"open stage {stageSwipe.currentStateIndex}");
            GameData.currentStage = stageSwipe.currentStateIndex + 1;
            revertFadeASyncLoading.PlayRevertAndLoadDefault();
            // SceneManager.LoadScene("LevelScreen");
        }
        else Debug.Log("Still locked");
    }

    void UnlockStage(int stageID)
    {
        Debug.Log("Unlocked");
        Debug.Log(panelAnimations[stageID-1]);
        panelAnimations[stageID-1].Running();
    }

}
