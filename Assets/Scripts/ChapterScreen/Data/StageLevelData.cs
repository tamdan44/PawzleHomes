[System.Serializable]
public class StageLevelData
{
    public string levelName;
    public int levelNumber;
    public int status; //-1 = Locked , 0 = not completed, 1 = completed, 2 = Fully completed
    public int score;
    public string Description; // Description of the level
    public string levelImage; // Path to the level image ()
    public StageLevelData()
    {
        levelName = "Default Level";
        levelNumber = 0;
        status = -1; // Default status is locked
        score = 0;
        Description = "This is a default level description.";
        levelImage = "default_level_image_path"; // Default image path
    }
}