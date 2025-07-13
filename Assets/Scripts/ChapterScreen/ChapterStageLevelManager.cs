using Assets.Scripts.SaveLoad;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ChapterScreen.Data
{
    public class ChapterStageLevelManager
    {
        private SaveSystem.SaveData _saveData;

        public ChapterStageLevelManager(SaveSystem.SaveData saveData)
        {
            _saveData = saveData;
        }
        // Kiểm tra xem có bất kỳ Chapter nào tồn tại không
        public bool checkAnyChapterExists()
        {
            // Kiểm tra xem danh sách chapters có phần tử không
            return _saveData.chapters.Count > 0;
        }
        // Tạo Chapter mới
        public void CreateNewChapter(int chapterNumber, string chapterName)
        {
            Debug.Log($"Tiến hành khởi tạo chapterNumber {chapterNumber}");
            // Kiểm tra xem Chapter đã tồn tại chưa
            if (_saveData.chapters.Exists(ch => ch.chapterNumber == chapterNumber))
            {
                Debug.LogWarning($"Chapter {chapterNumber} đã tồn tại.");
                return;
            }

            // Tạo Chapter mới
            ChapterData newChapter = new ChapterData()
            {
                chapterNumber = chapterNumber,
                chapterName = chapterName,
                chapterDescription = $"Chapter {chapterNumber} doesn't have any description yet",
                chapterImage = "Chapter {chapterNumber} doesn't have any Image please update to use",
                status = 0,
                stages = new List<ChapterStageData>()
            };


            _saveData.chapters.Add(newChapter);
            Debug.Log($"Đã thêm Chapter {chapterNumber}: {chapterName}");
        }

        // Tạo Stage mới trong Chapter
        public void CreateNewStage(int chapterNumber, int stageNumber, string stageName)
        {
            Debug.Log($"Tiến hành khởi tạo {stageNumber}");
            // Tìm Chapter theo chapterNumber
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter == null)
            {
                Debug.LogError($"Chapter {chapterNumber} không tồn tại.");
                return;
            }

            // Kiểm tra xem Stage đã tồn tại chưa
            if (chapter.stages.Exists(st => st.stageNumber == stageNumber))
            {
                Debug.LogWarning($"Stage {stageNumber} đã tồn tại trong Chapter {chapterNumber}.");
                return;
            }

            // Tạo Stage mới
            ChapterStageData newStage = new ChapterStageData()
            {
                stageNumber = stageNumber,
                status = 0,  // Chưa hoàn thành
                totalScore = "0",  // Điểm tổng ban đầu
                Levels = new List<StageLevelData>(),  // Danh sách level trong stage này
            };

            // Thêm Stage vào Chapter
            chapter.stages.Add(newStage);
            Debug.Log($"Đã tạo Stage {stageNumber}: {stageName} trong Chapter {chapterNumber}");
        }
        // Cập nhật lại LevelMenu từ dữ liệu đã lưu
        // Trong ChapterStageLevelManager.cs
        // ChapterStageLevelManager.cs
        public void Load(
            LevelButton buttonPrefab,
            Transform container,
            List<ChapterData> chapterDataList,
            List<LevelButton> outLevelButtons
        )
        {
            if (buttonPrefab == null || container == null)
            {
                Debug.LogError("Bạn phải truyền buttonPrefab và container vào Load().");
                return;
            }

            // Xóa hết UI cũ
            foreach (Transform t in container) GameObject.Destroy(t.gameObject);
            outLevelButtons.Clear();

            if (chapterDataList == null) return;
            foreach (var chapter in chapterDataList)
                foreach (var stage in chapter.stages)
                    foreach (var level in stage.Levels)
                    {
                        var btn = GameObject.Instantiate(buttonPrefab, container);
                        btn.levelNumber = level.levelNumber;
                        btn.levelCleared = level.status == 1;
                        btn.fullCleared = level.status == 2;
                        btn.levelUnlocked = level.status != -1;
                        btn.stageNumber = stage.stageNumber;
                        outLevelButtons.Add(btn);
                        btn.InitializeUI();

                    }
        }




        // Thêm Chapter mới vào danh sách
        public void AddChapter(ChapterData newChapter)
        {
            if (_saveData.chapters.Exists(ch => ch.chapterNumber == newChapter.chapterNumber))
            {
                Debug.LogWarning("Chapter đã tồn tại.");
                return;
            }
            _saveData.chapters.Add(newChapter);
        }

        // Cập nhật thông tin của Chapter
        public void UpdateChapter(int chapterNumber, ChapterData updatedChapter)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                chapter.chapterName = updatedChapter.chapterName;
                chapter.status = updatedChapter.status;
                chapter.chapterDescription = updatedChapter.chapterDescription;
            }
            else
            {
                Debug.LogError($"Không tìm thấy Chapter {chapterNumber}.");
            }
        }

        // Xóa Chapter
        public void RemoveChapter(int chapterNumber)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                _saveData.chapters.Remove(chapter);
            }
            else
            {
                Debug.LogError($"Không tìm thấy Chapter {chapterNumber} để xóa.");
            }
        }

        // Thêm Stage vào Chapter
        public void AddStageToChapter(int chapterNumber, ChapterStageData newStage)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                if (chapter.stages.Exists(st => st.stageNumber == newStage.stageNumber))
                {
                    Debug.LogWarning($"Stage {newStage.stageNumber} đã tồn tại trong Chapter {chapterNumber}.");
                    return;
                }
                chapter.stages.Add(newStage);
            }
            else
            {
                Debug.LogError($"Không tìm thấy Chapter {chapterNumber}.");
            }
        }

        // Cập nhật Stage trong Chapter
        public void UpdateStage(int chapterNumber, int stageNumber, ChapterStageData updatedStage)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                ChapterStageData stage = chapter.stages.Find(st => st.stageNumber == stageNumber);
                if (stage != null)
                {
                    stage.status = updatedStage.status;
                    stage.totalScore = updatedStage.totalScore;
                    stage.Levels = updatedStage.Levels; // Cập nhật các level trong stage
                }
                else
                {
                    Debug.LogError($"Không tìm thấy Stage {stageNumber} trong Chapter {chapterNumber}.");
                }
            }
            else
            {
                Debug.LogError($"Không tìm thấy Chapter {chapterNumber}.");
            }
        }

        // Xóa Stage khỏi Chapter
        public void RemoveStageFromChapter(int chapterNumber, int stageNumber)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                ChapterStageData stage = chapter.stages.Find(st => st.stageNumber == stageNumber);
                if (stage != null)
                {
                    chapter.stages.Remove(stage);
                }
                else
                {
                    Debug.LogError($"Không tìm thấy Stage {stageNumber} trong Chapter {chapterNumber}.");
                }
            }
        }

        // Thêm Level vào Stage trong Chapter
        public void AddLevelToStage(int chapterNumber, int stageNumber, int status, int score, string description, string levelImage)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                ChapterStageData stage = chapter.stages.Find(st => st.stageNumber == stageNumber);
                if (stage != null)
                {
                    StageLevelData newLevel = new StageLevelData()
                    {
                        levelNumber = stage.Levels.Count + 1,
                        status = status,
                        score = score,
                    };
                    stage.Levels.Add(newLevel);

                }
                else
                {
                    Debug.LogError($"Không tìm thấy Stage {stageNumber} trong Chapter {chapterNumber}.");
                }
            }
        }

        // Cập nhật Level trong Stage
        public void UpdateLevel(int chapterNumber, int stageNumber, int levelNumber, int status, int score, string description)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                ChapterStageData stage = chapter.stages.Find(st => st.stageNumber == stageNumber);
                if (stage != null)
                {
                    StageLevelData level = stage.Levels.Find(lv => lv.levelNumber == levelNumber);
                    if (level != null)
                    {
                        level.status = status;
                        level.score = score;
                    }
                    else
                    {
                        Debug.LogError($"Không tìm thấy Level {levelNumber} trong Stage {stageNumber}.");
                    }
                }
                else
                {
                    Debug.LogError($"Không tìm thấy Stage {stageNumber} trong Chapter {chapterNumber}.");
                }
            }

        }

        // Xóa Level trong Stage
        public void RemoveLevelFromStage(int chapterNumber, int stageNumber, int levelNumber)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                ChapterStageData stage = chapter.stages.Find(st => st.stageNumber == stageNumber);
                if (stage != null)
                {
                    StageLevelData level = stage.Levels.Find(lv => lv.levelNumber == levelNumber);
                    if (level != null)
                    {
                        stage.Levels.Remove(level);
                    }
                    else
                    {
                        Debug.LogError($"Không tìm thấy Level {levelNumber} trong Stage {stageNumber}.");
                    }
                }
            }
        }

        // Lấy Chapter theo chapterNumber
        public ChapterData GetChapter(int chapterNumber)
        {
            return _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
        }

        // Lấy Stage trong Chapter theo chapterNumber và stageNumber
        public ChapterStageData GetStageInChapter(int chapterNumber, int stageNumber)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            return chapter?.stages.Find(st => st.stageNumber == stageNumber);
        }

        // Lấy Level trong Stage theo chapterNumber, stageNumber, và levelNumber
        public StageLevelData GetLevelInStage(int chapterNumber, int stageNumber, int levelNumber)
        {
            ChapterData chapter = _saveData.chapters.Find(ch => ch.chapterNumber == chapterNumber);
            if (chapter != null)
            {
                ChapterStageData stage = chapter.stages.Find(st => st.stageNumber == stageNumber);
                return stage?.Levels.Find(lv => lv.levelNumber == levelNumber);
            }
            return null;
        }
    }


}
