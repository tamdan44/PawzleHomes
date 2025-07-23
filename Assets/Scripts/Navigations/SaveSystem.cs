// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;
// using UnityEngine;

// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////              🚨 CẢNH BÁO 🚨                      /////////////////
// ///////////////////////////            ⚠️ ĐÂY LÀ FILE QUAN TRỌNG ⚠️          /////////////////
// ///////////////////////////       ❗❗❗ KHÔNG ĐƯỢC XÓA ❗❗❗             //////////////////
// /////////////////////////// Nếu xóa sẽ không thể lưu và tải dữ liệu của game //////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////////////////////
// namespace Assets.Scripts.SaveLoad
// {
//     public class SaveSystem
//     {
//         private static SaveData _saveData = new SaveData();

//         [System.Serializable]
//         public class SaveData
//         {
//             public List<ChapterData> chapters = new List<ChapterData>();
//         }
//         public static SaveData GetSaveData()
//         {
//             return _saveData;
//         }

//         public static string SaveFileName()
//         {
//             string saveFile = Application.persistentDataPath + "/gameData.json";
//             return saveFile;
//         }
//         public static bool CheckFileExists()
//         {
//             return File.Exists(SaveFileName());
//         }

//         #region Save
//         public static void Save()
//         {
//             HandleSaveData();
//             File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
//         }


//         public static async Task SaveAsync()
//         {
//             HandleSaveData();
//             string saveContent = JsonUtility.ToJson(_saveData, true);
//             await File.WriteAllTextAsync(SaveFileName(), saveContent);
//         }


//         private static void HandleSaveData()
//         {
//             if (GameManager.Instance?.LevelMenu == null)
//             {
//                 Debug.LogError("SaveSystem: Missing GameManager.Instance.LevelMenu!");
//                 return;
//             }

//             if (GameManager.Instance.LevelMenu.levelButtons == null)
//                 GameManager.Instance.LevelMenu.levelButtons = new List<LevelButtonNew>();

//             _saveData.chapters.Clear();  // Xóa dữ liệu cũ trước khi lưu mới
//             // Lưu Chapter, Stage, Level
//             ChapterData newChapter = new ChapterData()
//             {
//                 chapterNumber = 1,
//                 chapterName = "Chapter 1",
//                 status = 2,  // Fully completed
//                 chapterImage = "path_to_image",
//                 chapterDescription = "This is a detailed description of Chapter 1",
//                 stages = new List<ChapterStageData>()
//             };

//             ChapterStageData newStage = new ChapterStageData()
//             {
//                 stageNumber = 1,
//                 status = 1,  // Completed
//                 totalScore = "1500",
//                 Levels = new List<StageLevelData>()
//             };

//             foreach (var button in GameManager.Instance.LevelMenu.levelButtons)
//             {
//                 StageLevelData newLevel = new StageLevelData()
//                 {
//                     levelNumber = button.levelNumber,
//                     status = button.fullCleared ? 2 : button.levelCleared ? 1 : button.levelUnlocked ? 0 : -1,   // Trạng thái level
//                     score = button.fullCleared ? 100 : button.levelCleared ? 50 : 0,  // Điểm sao của level
//                 };
//                 newStage.Levels.Add(newLevel);
//             }

//             newChapter.stages.Add(newStage);
//             _saveData.chapters.Add(newChapter);
//         }
//         #endregion

//         #region Load
//         public static void Load()
//         {
//             if (File.Exists(SaveFileName()))
//             {
//                 string saveContent = File.ReadAllText(SaveFileName());
//                 SaveData loaded = JsonUtility.FromJson<SaveData>(saveContent);
//                 // Giữ object cũ, chỉ copy nội dung vào
//                 _saveData.chapters.Clear();
//                 _saveData.chapters.AddRange(loaded.chapters);
//                 //_saveData = JsonUtility.FromJson<SaveData>(saveContent);
//                 //HandleLoadData();
//             }
//             else
//             {
//                 Debug.LogWarning("Save file not found, creating a new one.");
//                 CreateDefaultSaveFile();
//             }
//         }


//         public static async Task LoadAsync()
//         {
//             if (File.Exists(SaveFileName()))
//             {
//                 string saveContent = await File.ReadAllTextAsync(SaveFileName());
//                 _saveData = JsonUtility.FromJson<SaveData>(saveContent);
//                 HandleLoadData();
//             }
//             else
//             {
//                 Debug.LogWarning("Save file not found, creating a new one.");
//                 CreateDefaultSaveFile();
//             }
//         }


//         private static void HandleLoadData()
//         {
//             if (GameManager.Instance == null || GameManager.Instance.LevelMenu == null)
//             {
//                 Debug.LogError("SaveSystem: GameManager or LevelMenu is missing!");
//                 return;
//             }
//             foreach (var item in _saveData.chapters)
//             {
//                 foreach (var stage in item.stages)
//                 {
//                     foreach (var level in stage.Levels)
//                     {
//                         LevelButton levelButton = GameManager.Instance.LevelMenu.levelButtons.Find(btn => btn.levelNumber == level.levelNumber);
//                         if (levelButton != null)
//                         {
//                             levelButton.Load(level);
//                             levelButton.gameObject.SetActive(true);
//                             levelButton.InitializeUI();


//                         }
//                         else
//                         {
//                             Debug.LogWarning($"Level {level.levelNumber} not found in LevelMenu.");
//                         }
//                     }
//                 }

//             }

//         }



//         private static void CreateDefaultSaveFile()
//         {

//             _saveData = new SaveData();
//             Save();
//         }
//         #endregion

//     }
// }
