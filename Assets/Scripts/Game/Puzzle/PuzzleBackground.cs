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
    private GameObject[] bgList;

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
        bgList = GetComponentsInChildren<Transform>(true).Where(t => t != transform).Select(t => t.gameObject).ToArray();
        Debug.Log($"bg count {bgList.Length}");

        Color colored = grid.GetComponent<Image>().color;
        colored.a = 0f;
        grid.GetComponent<Image>().color = colored;
        for (int i = GameData.currentLevel; i < bgList.Length; i++)
        {
            // Color coloring = bgList[i].GetComponent<Image>().color;
            // coloring.a = 0f;
            bgList[i].GetComponent<Image>().color = colored;
        }

        SaveSystem.ConvertImageColor(ring1.GetComponent<Image>(), GameData.shapeColor);
        Color ringColor = grid.GetComponent<Image>().color;
        ringColor.a = 0.8f;
        ring1.GetComponent<Image>().color = ringColor;
    }

    void Start()
    {
        for (int i = 0; i < GameData.currentLevel; i++)
        {
            Color coloring = bgList[i].GetComponent<Image>().color;
            coloring.a = 0.1f;
            bgList[i].GetComponent<Image>().color = coloring;
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
        yield return StartCoroutine(Disappear(grid.GetComponent<Image>(), 0.5f, 1f, 0));
        yield return StartCoroutine(Disappear(bgList[GameData.currentLevel].GetComponent<Image>(), 1f, 0f, 1f));
        yield return new WaitForSeconds(1.1f);
        Debug.Log("finished");
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
