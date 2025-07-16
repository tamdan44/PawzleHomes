using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
    private static string filePath = "Assets/Resources/levels.json";

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
        string filePath = "Assets/Resources/levels.json";
        
        if (File.Exists(filePath) && GameData.levelDB == null)
        {
            string json = File.ReadAllText(filePath);
            GameData.levelDB = JsonUtility.FromJson<LevelDatabase>(json);
            Debug.Log($"File.Exists {filePath}");
        }

        if (File.Exists(saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveDataPath, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            GameData.playerLevelData = data.playerLevelData;
            GameData.stageUnlocked = data.stageUnlocked;
        }
        else
        {
            Debug.Log("Save file not found in" + saveDataPath);

            GameData.playerLevelData = new();
            GameData.playerLevelData[(1, 1)] = 0;
            GameData.stageUnlocked = new bool[9];
            GameData.stageUnlocked[0] = true;
        }
    }
}

