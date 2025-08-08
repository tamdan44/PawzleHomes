using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;
// using Mono.Cecil;
// using UnityEditor.PackageManager;

// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////              🚨 CẢNH BÁO 🚨                      /////////////////
// ///////////////////////////            ⚠️ ĐÂY LÀ FILE QUAN TRỌNG ⚠️          /////////////////
// ///////////////////////////       ❗❗❗ KHÔNG ĐƯỢC XÓA ❗❗❗             //////////////////
// /////////////////////////// Nếu xóa sẽ không thể lưu và tải dữ liệu của game //////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
/// 
[UnityEngine.Scripting.Preserve]
public class SaveSystem: MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        GameEvents.SavePlayer += SavePlayer;
        GameEvents.LoadPlayer += LoadPlayer;
        GameEvents.LoadNewPlayer += LoadNewPlayer;
        GameEvents.UnlockAllLevelsInStage += UnlockAllLevelsInStage;
    }

    void OnDisable()
    {
        GameEvents.SavePlayer -= SavePlayer;
        GameEvents.LoadPlayer -= LoadPlayer;
        GameEvents.LoadNewPlayer -= LoadNewPlayer;
        GameEvents.UnlockAllLevelsInStage -= UnlockAllLevelsInStage;
    }

    public void SavePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";

        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void LoadPlayer()
    {
        string saveDataPath = Application.persistentDataPath + "/player.fun";

        if (File.Exists(saveDataPath))
        {
                    Debug.Log(saveDataPath);

            LoadOldPlayer(saveDataPath);
        }   
        else
        {
            LoadNewPlayer();
        }
        
    }

    public void LoadOldPlayer(string dataPath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(dataPath, FileMode.Open);
        PlayerData data = formatter.Deserialize(stream) as PlayerData;
        stream.Close();

        GameData.playerLevelData = data.playerLevelData;
        GameData.stageUnlocked = data.stageUnlocked;
        GameData.playerBigCoins = data.playerBigCoins;
        GameData.playerCoins = data.playerCoins;
        GameData.numHint = data.numHint;
        GameData.AdBlock = data.adBlock;

        LoadLevelResources();
    }

    public void LoadNewPlayer()
    {
        LoadLevelResources();

        GameData.playerLevelData = new();

        GameData.stageUnlocked = new bool[6];
        GameData.stageUnlocked[0] = true;

        UnlockAllLevelsInStage(1, 0); // unlock stage 1
        UnlockAllLevelsInStage(2, -1);
        UnlockAllLevelsInStage(3, -1);

        GameData.playerBigCoins = 0;
        GameData.playerCoins = 0;
        GameData.numHint = 3;
        GameData.AdBlock = false;

    }

    static void LoadLevelResources()
    {
        // string filePath = "Assets/Resources/levels.json";

        if (GameData.levelDB == null)
        {
            string json = Resources.Load<TextAsset>("levels").text;
            GameData.levelDB = JsonUtility.FromJson<LevelDatabase>(json);
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

    public void UnlockAllLevelsInStage(int stageID, int status)
    {
        for (int i = 1; i <= GameData.stageLevelDict[stageID]; i++)
        {
            GameData.playerLevelData[(stageID, i)] = status;
        }
    }


}

