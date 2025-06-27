using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{

    void Start()
    {

    }

    public void OpenLevel(string text)
    {

        var match = Regex.Match(text, @"stage(?<stageID>\d+)\s+level(?<levelID>\d+)");
        if (match.Success)
        {
            int stageID = int.Parse(match.Groups["stageID"].Value);
            int levelID = int.Parse(match.Groups["levelID"].Value);

            Debug.Log($"Stage ID: {stageID}");
            Debug.Log($"Level ID: {levelID}");

            GameData.currentStage = stageID;
            GameData.currentLevel = levelID;

            SceneManager.LoadScene("PuzzlePlay");
        }
        else
        {
            Debug.Log("Input string format is invalid.");
        }



    }
}
