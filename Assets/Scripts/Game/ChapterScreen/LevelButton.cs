// using Assets.Scripts.SaveLoad;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class LevelButton : MonoBehaviour, IPointerClickHandler
// {
//     [Header("Stars and Texts")]
//     public List<Starr> starList = new List<Starr>();
//     public GameObject inactiveText; // Shown when locked
//     public GameObject activeText;   // Shown when cleared

//     [Header("Number Images")]
//     public List<Image> inactiveNumbers; // Visible when not cleared
//     public List<Image> activeNumbers;   // Visible when cleared

//     [Header("Level Info")]
//     public int levelNumber;
//     public bool levelCleared;
//     public bool fullCleared;
//     public bool levelUnlocked;
//     public int stageNumber;

//     // Registry for unlocking logic
//     private static List<LevelButton> allLevelButtons = new List<LevelButton>();

//     private void Awake()
//     {
//         allLevelButtons.Add(this);
//     }

//     private void OnDestroy()
//     {
//         allLevelButtons.Remove(this);
//     }

//     private void Start()
//     {
//         InitializeUI();
//     }

//     /// <summary>
//     /// Set up visuals based on state (locked, cleared, etc.)
//     /// </summary>
//     public void InitializeUI()
//     {
//         // Set visibility based on unlocked state
//         gameObject.SetActive(true);


//         if (levelCleared || fullCleared)
//         {
//             activeText?.SetActive(true);
//             inactiveText?.SetActive(false);
//             // If cleared, show active numbers
//             foreach (var img in activeNumbers)
//             {
//                 img.enabled = true;
//             }
//         }
//         else
//         {
//             activeText?.SetActive(false);
//             inactiveText?.SetActive(true);
//             // If not cleared, show inactive numbers
//             foreach (var img in inactiveNumbers)
//             {
//                 img.enabled = true;
//             }
//         }

//         // Update number sprites and enable correct images
//         UpdateNumberImages();

//         // Reset all stars
//         foreach (var star in starList)
//             star.SetStarInactive();

//         // Activate stars and next unlock if needed
//         if (levelCleared || fullCleared)
//             ActivateStars(confirmSave: false);
//     }

//     private void UpdateNumberImages()
//     {
//         string spriteName = $"{levelNumber}_brown";
//         Sprite levelSprite = Resources.Load<Sprite>($"Sprites/LevelScreen/{spriteName}");
//         if (levelSprite == null)
//         {
//             Debug.LogError($"LevelButton: Sprite '{spriteName}' not found.");
//             return;
//         }

//         // Colors
//         Color inactiveColor = levelUnlocked && !levelCleared
//             ? new Color(0.7f, 0f, 0.5f, 1f)
//             : new Color(1f, 1f, 1f, 0.6f);
//         Color activeColor = new Color(0f, 1f, 0.5f, 1f);

//         // Inactive images visible when not cleared
//         foreach (var img in inactiveNumbers)
//         {
//             img.sprite = levelSprite;
//             img.color = inactiveColor;
//             img.enabled = !levelCleared;
//             if (levelNumber > 9)
//                 img.rectTransform.sizeDelta = new Vector2(110f, 110f);
//         }
//         // Active images visible when cleared
//         foreach (var img in activeNumbers)
//         {
//             img.sprite = levelSprite;
//             img.color = activeColor;
//             img.enabled = levelCleared;
//             if (levelNumber > 9)
//                 img.rectTransform.sizeDelta = new Vector2(110f, 110f);
//         }
//     }

//     public void OnPointerClick(PointerEventData eventData)
//     {
//         if (!levelUnlocked)
//         {
//             Debug.Log($"Level {levelNumber} is locked.");
//             return;
//         }
//         ActivateStars();

//         // Mark this level as cleared on click
//         levelCleared = true;
//         // Optionally set fullCleared based on some condition (e.g., all stars)
//         // fullCleared = (StarCount() == starList.Count);
//         // Refresh UI and save state
//         InitializeUI();
//         SaveGameState();

//         GameEvents.OpenLevel(stageNumber, levelNumber);
//     }

//     /// <summary>
//     /// Count active stars
//     /// </summary>
//     public int StarCount()
//     {
//         int count = 0;
//         foreach (var star in starList)
//             if (star.IsActive()) count++;
//         return count;
//     }

//     /// <summary>
//     /// Activate star visuals and unlock next level
//     /// </summary>
//     private void ActivateStars(bool confirmSave = true)
//     {
//         if (!levelCleared)
//             return;

//         // Always activate the first star
//         if (starList.Count > 0)
//             starList[0].SetStarActive();

//         if (fullCleared)
//         {
//             // Activate all stars
//             foreach (var star in starList)
//                 star.SetStarActive();
//         }

//         // Unlock next level
//         UnlockNextLevel();

//         // Optionally save after unlocking
//         if (confirmSave)
//             SaveGameState();
//     }

//     private void UnlockNextLevel()
//     {
//         int nextLevel = levelNumber + 1;
//         var nextBtn = allLevelButtons.Find(b =>
//             b.levelNumber == nextLevel &&
//             b.stageNumber == stageNumber);

//         if (nextBtn != null)
//         {
//             nextBtn.levelUnlocked = true;
//             nextBtn.InitializeUI();
//         }
//     }

//     private void SaveGameState()
//     {
//         // Assuming a central manager handles saving all levels
//         SaveSystem.Save();
//     }

//     #region Save and Load for manager
//     public void Save(ref StageLevelData data)
//     {
//         data.levelNumber = levelNumber;
//         data.status = fullCleared ? 2 : levelCleared ? 1 : levelUnlocked ? 0 : -1;
//         data.score = StarCount();
//     }

//     public void Load(StageLevelData data)
//     {
//         levelNumber = data.levelNumber;
//         levelCleared = (data.status == 1);
//         fullCleared = (data.status == 2);
//         levelUnlocked = (data.status != -1);
//         InitializeUI();
//     }
//     #endregion
// }
