using UnityEngine;
using TMPro;

public class UploadBugs : MonoBehaviour
{
    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private PanelAnimation panelAnimation;
    [SerializeField] private StagePlayer stagePlayer;
    [SerializeField] private StageSwipe stageSwipe;
    void Start()
    {
        if (!stagePlayer.enabled)
        {
            Debug.LogWarning("StagePlayer was disabled — enabling now.");
            stagePlayer.enabled = true;
            foreach (var panel in stagePlayer.panelAnimations)
            {
                panel.enabled = true;
            }

        }


        string errors = $"{GameData.stageUnlocked == null}" + $"{GameData.stageUnlocked.Length}" + $"{GameData.stageTransition}" + $"{stagePlayer.isActiveAndEnabled}";
        tMP_Text.text = errors;

        // if (GameData.stageTransition > 1)
        //     stagePlayer.InitializeStages();


        if (stagePlayer.panelTest > 0)
        {
            errors += $"panelTest: {stagePlayer.panelTest} ";
        }
        if (stagePlayer.isActiveAndEnabled)
        {
            errors += "stagePlayer ";
        }
        // foreach (bool unlocked in GameData.stageUnlocked)
        // {
        //     if (unlocked)
        //     {
        //         errors += "unlock";
        //     }
        //     else
        //     {
        //         errors += "0";
        //     }
        // }

        // if (panelAnimation.panelRan)
        // {
        //     errors += "panelRan ";
        // }

        tMP_Text.text = errors;
        if (!panelAnimation.panelRan)
        {
            if (stagePlayer.panelAnimations.Length > 0)
                stagePlayer.panelAnimations[0].InitializeStageUnlocked();
        }

    }

    void Update()
    {
        if (stageSwipe.currentStateIndex > 0)
        {
            // tMP_Text.text = stageSwipe.currentStateIndex.ToString();
            string saveDataPath = Application.persistentDataPath + "/player.fun" + stageSwipe.currentStateIndex.ToString();
            tMP_Text.text = saveDataPath;
            tMP_Text.text = saveDataPath + SaveSystem.CountNumberOfClearedLevels(GameData.currentStage+1).ToString();

        }

        
    }
}
