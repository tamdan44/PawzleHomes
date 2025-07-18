[System.Serializable]
public class StageLevelData
{
    public int levelNumber;
    public int status; //-1 = Locked , 0 = not completed, 1 = completed, 2 = Fully completed
    public int score;
    public StageLevelData()
    {
        levelNumber = 0;
        status = -1; // Default status is locked
        score = 0;
    }
}