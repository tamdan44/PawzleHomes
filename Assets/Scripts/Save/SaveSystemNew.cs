using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using System.Linq;

// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////              🚨 CẢNH BÁO 🚨                      /////////////////
// ///////////////////////////            ⚠️ ĐÂY LÀ FILE QUAN TRỌNG ⚠️          /////////////////
// ///////////////////////////       ❗❗❗ KHÔNG ĐƯỢC XÓA ❗❗❗             //////////////////
// /////////////////////////// Nếu xóa sẽ không thể lưu và tải dữ liệu của game //////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
public static class SaveSystem
{

    public static void SavePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadPlayer()
    {
        string saveDataPath = Application.persistentDataPath + "/player.fun";

        if (File.Exists(saveDataPath))
        {
            LoadLevelResources();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveDataPath, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            GameData.playerLevelData = data.playerLevelData;
            GameData.stageUnlocked = data.stageUnlocked;
            GameData.playerBigCoins = data.playerBigCoins;
            GameData.playerCoins = data.playerCoins;
            GameData.numHint = data.numHint;
        }
        else
        {
            LoadNewPlayer();
        }
    }

    public static void LoadNewPlayer()
    {
        LoadLevelResources();
        Debug.Log("Save file not found in");

        GameData.playerLevelData = new();
        GameData.playerLevelData[(1, 1)] = 0;
        GameData.stageUnlocked = new bool[6];
        GameData.stageUnlocked[0] = true;

        GameData.playerBigCoins = 0;
        GameData.playerCoins = 0;
        GameData.numHint = 3;
    }

    static void LoadLevelResources()
    {
        string filePath = "Assets/Resources/levels.json";

        if (File.Exists(filePath) && GameData.levelDB == null)
        {
            string json = File.ReadAllText(filePath);
            GameData.levelDB = JsonUtility.FromJson<LevelDatabase>(json);
            Debug.Log($"File.Exists {filePath}");
        }

        int maxStageID = GameData.levelDB.levels.Max(item => item.stageID);
        GameData.stageLevelDict = new();
        for (int i = 1; i <= maxStageID; i++)
        {
            GameData.stageLevelDict[i] = GameData.levelDB.levels.Count(item => item.stageID == i);
        }

    }

    // Dictionary<string, Color> stringToColor ={
    //     'white': Color.white,
    // };

    public static void ConvertImageColor(UnityEngine.UI.Image img, string imgColor)
    {
        if (ColorUtility.TryParseHtmlString(imgColor, out Color newColor))
        {
            img.color = newColor;
        }
        else
        {
            Debug.Log("there are no valid color/ cannot found ");
        }
    }

}

