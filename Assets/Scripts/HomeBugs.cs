using UnityEngine;
using TMPro;
using UnityEditor;

public class HomeBugs : MonoBehaviour
{
    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private MenuButtons menuButtons;
    // [SerializeField] private StagePlayer stagePlayer;
    // [SerializeField] private StageSwipe stageSwipe;
    void Start()
    {
        string errors = "";
        TextErrors(errors, $"s{GameData.currentStage} l{GameData.currentLevel}");
    }

    void Update()
    {
        string errors = "";
        TextErrors(errors, $"s{GameData.currentStage} l{GameData.currentLevel}");
        TextErrors(errors, $"{menuButtons.isActiveAndEnabled} {menuButtons.enabled}");

    }

    void TextErrors(string err, string additional_text)
    {
        err += additional_text;
        tMP_Text.text = err;
    }

}
