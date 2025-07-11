using System.Collections.Generic;

[System.Serializable]
public class ChapterData
{

    public string chapterName;
    public int chapterNumber;
    public int status; //-1 = Locked , 0 = not completed, 1 = completed, 2 = Fully completed
    public string chapterImage; // Path to the chapter image
    public string chapterDescription; // Description of the chapter
    public List<ChapterStageData> stages;

    public ChapterData()
    {
        chapterNumber = 0;
        chapterName = "Default Chapter";
        status = -1; // Default status is locked
        chapterImage = "default_image_path"; // Default image path
        chapterDescription = "This is a default chapter description.";
        stages = new List<ChapterStageData>();

    }

}

