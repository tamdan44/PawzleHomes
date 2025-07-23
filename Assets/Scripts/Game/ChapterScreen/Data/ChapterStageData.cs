
using System.Collections.Generic;

[System.Serializable]
public class ChapterStageData
{
    public int stageNumber;
    public int status; //-1 = Locked , 0 = not completed, 1 = completed, 2 = Fully completed
    public string totalScore;
    public List<StageLevelData> Levels;
    public ChapterStageData()
    {
        stageNumber = 0;
        status = -1; // Default status is locked
        totalScore = "0";
        Levels = new List<StageLevelData>();
    }

}
