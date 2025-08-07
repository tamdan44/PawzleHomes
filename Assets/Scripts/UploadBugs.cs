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
            panelAnimation.enabled = true;
            
        }


        string errors = $"{stagePlayer.panelAnimations.Length > 0}" + $"{stagePlayer.panelAnimations == null}" + $"{stagePlayer.enabled}" + $"{stagePlayer.isActiveAndEnabled}";
            if (!panelAnimation.panelRan)
            {
                if(stagePlayer.panelAnimations.Length > 0)
                stagePlayer.panelAnimations[0].InitializeStageUnlocked();
            }


        if (stagePlayer.panelTest >= 0)
        {
            errors += $"panelTest: {stagePlayer.panelTest} ";
        }
        if (stagePlayer.isActiveAndEnabled)
        {
            errors += "stagePlayer ";
        }

        // if (panelAnimation.panelRan)
        // {
        //     errors += "panelRan ";
        // }

        tMP_Text.text = errors;
    }
}
