using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBackground : MonoBehaviour
{
    public Transform grid;
    public Transform ring1;
    public Transform ring2;
    public bool canActivate;
    private GameObject[] bgList;

    private void OnEnable()
    {
        GameEvents.GameOver += Run;
    }
    private void OnDisable()
    {
        GameEvents.GameOver -= Run;
    }

    void Awake()
    {
        canActivate = false;
        bgList = GetComponentsInChildren<Transform>(true).Where(t => t != transform).Select(t => t.gameObject).ToArray();
        Debug.Log($"bg count {bgList.Length}");
        grid.gameObject.SetActive(true);
        for (int i = 0; i < bgList.Length; i++)
        {
            Debug.Log("clearing");
            bgList[i].SetActive(true);
        }
        StartCoroutine(Execute());
    }
    void Start()
    {
        for (int i = 0; i < GameData.currentLevel; i++)
        {
            Debug.Log($"currently in the  {i}");
            bgList[i].SetActive(true);
        }
    }

    private void Run(int level)
    {
        StartCoroutine(Execute());
        canActivate = true;
    }

    private IEnumerator Execute()
    {
        yield return StartCoroutine(Resize(ring1, Vector2.one * 2, 0.3f));
        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(Resize(ring1, Vector2.one / 10, 0.25f));
        StartCoroutine(Resize(ring1, Vector2.one * 50, 1.5f));
        yield return new WaitForSeconds(0.2f);
        ring1.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(2f);
        StartCoroutine(Disappear(ring1.transform.parent, 1f, 1f, 0f));
        GameData.currentLevel++;
        Debug.Log(GameData.currentLevel - 1);
        Debug.Log(bgList[GameData.currentLevel - 1].GetComponent<Transform>());
        yield return StartCoroutine(Disappear(bgList[GameData.currentLevel - 1].transform, 1f, 0f, 1f));
        //bgList[GameData.currentLevel - 1].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Debug.Log("finished");
        //yield return StartCoroutine(Disappear(ring1.transform.parent, 1f, 0f, 1f));
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

    private IEnumerator Disappear(Transform _transform, float moveDuration, float currentAlpha, float expectedAlpha)
    {
        float elapsedTime = 0;
        while ( elapsedTime < moveDuration)
        {
            float alpha = Mathf.Lerp(currentAlpha, expectedAlpha, elapsedTime / moveDuration);
            _transform.GetComponent<Image>().canvasRenderer.SetAlpha(alpha);
            if (elapsedTime >= moveDuration) break;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("runnig");
        _transform.GetComponent<Image>().canvasRenderer.SetAlpha(expectedAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
