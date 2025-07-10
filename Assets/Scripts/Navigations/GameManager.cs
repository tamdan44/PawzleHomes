using Assets.Scripts.SaveLoad;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return null;
            }

            if (instance == null)
            {
                Instantiate(Resources.Load<GameManager>("GameManager"));
            }
#endif
            return instance;
        }
    }
    public LevelButton LevelButton { get; set; }
    public LevelMenu LevelMenu { get; set; }
    public bool _isLoading;
    public bool _isSaving;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (LevelMenu == null)
                LevelMenu = Object.FindFirstObjectByType<LevelMenu>();
            if (LevelButton == null)
                LevelButton = Object.FindFirstObjectByType<LevelButton>();
        }
        else
        {
            Destroy(gameObject);
        }
        SaveSystem.Load(); // Load the save data at the start
    }

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && !_isSaving)
        {
            Debug.Log("Saving game...");
            SaveSystem.Save();
            Debug.Assert(SaveSystem.SaveFileName() != null, "Save file name is null. Please check SaveSystem implementation.");
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && !_isLoading)
        {
            Debug.Log("Loading game...");
            SaveSystem.Load();
            Debug.Assert(SaveSystem.SaveFileName() != null, "Save file name is null. Please check SaveSystem implementation.");
        }
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            SaveSystem.Save();
            instance = null;
        }
    }

    //private async void SaveAsync()
    //{
    //    _isSaving = true;
    //    await SaveSystem.SaveAsynchronously();
    //    _isSaving = false;
    //}

    //private async void LoadAsync()
    //{
    //    _isLoading = true;
    //    await SaveSystem.LoadAsync();
    //    _isLoading = false;
    //}
}
