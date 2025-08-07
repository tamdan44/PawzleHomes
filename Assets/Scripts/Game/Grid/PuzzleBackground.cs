using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBackground : MonoBehaviour
{
    [SerializeField] private GameOver gameOver;
    [SerializeField] private Transform grid;
    [SerializeField] private Transform ring1;
    [SerializeField] private Transform ring2;
    [SerializeField] private Component[] components;
    private GameObject[] bgList;
    private bool[] levelCleareds = SaveSystem.GetBoolClearedLevels(GameData.currentStage);

    private void OnEnable()
    {
        GameEvents.LevelCleared += Run;
        GameEvents.GridAppears += RunGridAppears;
    }
    private void OnDisable()
    {
        GameEvents.LevelCleared -= Run;
        GameEvents.GridAppears -= RunGridAppears;
    }

    void Awake()
    {
        levelCleareds = SaveSystem.GetBoolClearedLevels(GameData.currentStage);

        Component component = components[Mathf.Max(0, GameData.currentStage - 1)];
        bgList = component.GetComponentsInChildren<Transform>(true).Where(t => t != component.transform).Select(t => t.gameObject).ToArray();

        bgList = bgList[0..GameData.stageLevelDict[GameData.currentStage]];
        for (int i = 0; i < components.Length; i++)
        {
            if (i != GameData.currentStage - 1)
            {
                components[i].gameObject.SetActive(false);
            }
        }

        Color colored = grid.GetComponent<Image>().color;
        colored.a = 0f;
        grid.GetComponent<Image>().color = colored;
        for (int i = 0; i < levelCleareds.Length; i++)
        {
                var image = bgList[i].GetComponent<Image>();
                if (image != null)
                {
                    image.color = colored;
                }
                else
                {
                    Debug.LogWarning($"No Image component on: {bgList[i].name}");
                }
        }

        SaveSystem.ConvertImageColor(ring1.GetComponent<Image>(), GameData.shapeColor);
        Color ringColor = grid.GetComponent<Image>().color;
        ringColor.a = 0.8f;
        ring1.GetComponent<Image>().color = ringColor;
    }

    void Start()
    {
        SetImagesAlpha(0.07f);
        // Color coloring = bgList[0].GetComponent<Image>().color;
        // coloring.a = 0.07f;
        // for (int i = 0; i < levelCleareds.Length; i++)
        // {
        //     if (levelCleareds[i])
        //     {
        //         bgList[i].GetComponent<Image>().color = coloring;
        //         Debug.Log($" i {i}");
        //     }
        // }
    }

    private void SetImagesAlpha(float alpha)
    {
        Color coloring = bgList[0].GetComponent<Image>().color;
        coloring.a = alpha;
        for (int i = 0; i < levelCleareds.Length; i++)
        {
            if (levelCleareds[i])
            {
                bgList[i].GetComponent<Image>().color = coloring;
                Debug.Log($" i {i}");
            }
        }
    }

    private void Run(int stars)
    {
        StartCoroutine(Execute(stars));
    }

    private IEnumerator Execute(int stars)
    {
        ring1.localScale = Vector2.zero;
        StartCoroutine(Disappear(ring1.GetComponent<Image>(), 0.5f, 0f, 1));
        yield return StartCoroutine(Resize(ring1, Vector2.one * 2, 0.3f));
        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(Resize(ring1, Vector2.one / 5, 0.2f));
        StartCoroutine(Resize(ring1, Vector2.one * 50, 1.2f));
        yield return new WaitForSeconds(0.3f);
        ring1.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.8f);
        SetImagesAlpha(1f);
        yield return StartCoroutine(Disappear(grid.GetComponent<Image>(), 0.5f, 1f, 0));

        yield return StartCoroutine(Disappear(bgList[GameData.currentLevel - 1].GetComponent<Image>(), 1f, 0f, 1f));
        yield return new WaitForSeconds(1.1f);
        gameOver.GameOverPopup(stars);
    }

    private void RunGridAppears()
    {
        StartCoroutine(GridAppears());
    }

    private IEnumerator GridAppears()
    {
        yield return StartCoroutine(Resize(grid, Vector2.one * 1.1f, 0f));
        StartCoroutine(Resize(grid, Vector2.one, 0.2f));
        yield return StartCoroutine(Disappear(grid.GetComponent<Image>(), 0.5f, 0f, 1));
    }


    private IEnumerator Resize(Transform _transform, Vector2 expectedScale, float moveDuration)
    {
        Vector2 currentScale = _transform.localScale;
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            float t = Mathf.Lerp(0f, 1f, elapsedTime / moveDuration);
            _transform.localScale = Vector2.Lerp(currentScale, expectedScale, t);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break; 
            yield return null;
        }
        _transform.localScale = expectedScale;
    }

    private IEnumerator Disappear(Image _image, float moveDuration, float currentAlpha, float expectedAlpha)
    {
        Color colored = _image.color;
        float elapsedTime = 0;
        while ( elapsedTime < moveDuration)
        {
            float alpha = Mathf.Lerp(currentAlpha, expectedAlpha, elapsedTime / moveDuration);
            colored.a = alpha;
            _image.color = colored;
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        Debug.Log("runnig");
        colored.a = expectedAlpha;
        _image.color = colored;
    }
}
