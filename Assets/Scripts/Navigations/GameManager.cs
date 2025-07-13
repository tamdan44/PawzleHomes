using UnityEngine;

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



    }
    private void Start()
    {
        // Initialize any game state or settings here
        // For example, load saved data or set initial values
        Debug.Log("GameManager started");

    }

    private void Update()
    {

    }




}
