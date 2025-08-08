using UnityEngine;
using TMPro;

public class StageOpen : MonoBehaviour
{
    [SerializeField] private StagePlayer stagePlayer;
    [SerializeField] private TMP_Text tMP_Text;
    void Start()
    {
        string e = "";
        if (GameData.stageUnlocked == null)
        {
            GameEvents.LoadPlayer();
            e += "no_gamedata ";
        }
        tMP_Text.text = e;
        if (!stagePlayer.enabled)
        {
            stagePlayer.enabled = true;
            e += "no_stagePlayer ";
        }
        tMP_Text.text = e;
        try
        {
            stagePlayer.InitializeStages();
            e += "initializestage";
        }
        catch
        {
            e += "no initializestage";
        }
        e += "hi";
        tMP_Text.text = e;
        // if (!stagePlayer.panelAnimations[0].panelRan)
        // {
        //     if (stagePlayer.panelAnimations.Length > 0)
        //         stagePlayer.panelAnimations[0].InitializeStageUnlocked();   
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
