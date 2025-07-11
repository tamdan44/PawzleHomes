using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class SaveSystem
    {
        private static SaveData _saveData = new SaveData
        {
            LevelDataList = new List<LevelData>(),  // Khởi tạo danh sách LevelData
        };

        [System.Serializable]
        public struct SaveData
        {
            public List<LevelData> LevelDataList;  // Lưu danh sách LevelData
        }

        public static string SaveFileName()
        {
            string saveFile = Application.persistentDataPath + "/save.save";
            return saveFile;
        }

        public static void Save()
        {
            HandleSaveData();
            File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
        }

        public static async Task SaveAsync()
        {
            HandleSaveData();
            string saveContent = JsonUtility.ToJson(_saveData, true);
            await File.WriteAllTextAsync(SaveFileName(), saveContent);
        }

        private static void HandleSaveData()
        {
            if (GameManager.Instance?.LevelMenu == null)
            {
                Debug.LogError("SaveSystem: Missing GameManager.Instance.LevelMenu!");
                return;
            }

            // Đảm bảo danh sách levelButtons không null
            if (GameManager.Instance.LevelMenu.levelButtons == null)
                GameManager.Instance.LevelMenu.levelButtons = new List<LevelButton>();

            // Lưu dữ liệu từ level buttons vào LevelDataList
            _saveData.LevelDataList.Clear();
            foreach (var button in GameManager.Instance.LevelMenu.levelButtons)
            {
                var levelData = new LevelData();
                button.Save(ref levelData);
                _saveData.LevelDataList.Add(levelData);
            }
        }

        public static void Load()
        {
            if (File.Exists(SaveFileName()))
            {
                string saveContent = File.ReadAllText(SaveFileName());
                _saveData = JsonUtility.FromJson<SaveData>(saveContent);
                HandleLoadData();
            }
            else
            {
                Debug.LogError("Save file not found.");
            }
        }

        //public static async Task LoadAsync()
        //{
        //    if (File.Exists(SaveFileName()))
        //    {
        //        string saveContent = await File.ReadAllTextAsync(SaveFileName());
        //        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        //        await HandleLoadDataAsync();
        //    }
        //    else
        //    {
        //        Debug.LogError("Save file not found.");
        //    }
        //}

        private static void HandleLoadData()
        {
            if (GameManager.Instance == null || GameManager.Instance.LevelMenu == null)
            {
                Debug.LogError("SaveSystem: GameManager or LevelMenu is missing!");
                return;
            }

            GameManager.Instance.LevelMenu.Load(_saveData.LevelDataList);
        }

        //private static async Task HandleLoadDataAsync()
        //{
        //    if (GameManager.Instance == null || GameManager.Instance.LevelMenu == null)
        //    {
        //        Debug.LogError("SaveSystem: GameManager or LevelMenu is missing!");
        //        return;
        //    }

        //    GameManager.Instance.LevelMenu.Load(_saveData.LevelDataList);
        //}
    }
}
