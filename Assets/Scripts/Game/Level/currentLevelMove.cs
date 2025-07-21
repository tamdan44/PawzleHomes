using UnityEngine;
using UnityEngine.UI;

public class currentLevelMove : MonoBehaviour
{
    private static currentLevelMove instance;
    [SerializeField] private LevelMenuNew levelMenuNew;
    [SerializeField] private MoveLevel moveLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        instance = this;
        if (instance != null)
        {
            DontDestroyOnLoad(instance);
            ToCurrentLevel();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void ToCurrentLevel()
    {
        for (int i = moveLevel.maxPage.Length; i == 0; i--)
        {
            Debug.Log("current page" + moveLevel.maxPage[i]);
            moveLevel.GetComponentInChildren<Scrollbar>().value = moveLevel.maxPage[i];
        }
    }
}
