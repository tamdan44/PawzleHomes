using UnityEngine;
using UnityEngine.UI;

public class StageTransitionCG : MonoBehaviour
{
    void OnEnable()
    {
        Sprite stageSprite = Resources.Load<Sprite>($"StageCG/chapter{GameData.currentStage-1}");
        this.GetComponent<Image>().sprite = stageSprite;
    }

}
