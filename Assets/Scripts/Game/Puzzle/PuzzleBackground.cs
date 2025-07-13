using System.Collections;
using System.Linq;
using UnityEngine;

public class PuzzleBackground : MonoBehaviour
{
    public Transform grid;
    public Transform ring1;
    public Transform ring2;
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
        bgList = GetComponentsInChildren<Transform>(true).Where(t => t != transform).Select(t => t.gameObject).ToArray();
        Debug.Log($"bg count {bgList.Length}");
        grid.gameObject.SetActive(true);
        for (int i = 0; i < bgList.Length; i++)
        {
            Debug.Log("clearing");
            bgList[i].SetActive(false);
        }
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
    }

    private IEnumerator Execute()
    {
        yield return StartCoroutine(Resize(ring1, Vector2.one * 2, 0.3f));
        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(Resize(ring1, Vector2.one / 10, 0.25f));
        StartCoroutine(Resize(ring1, Vector2.one * 50, 1.5f));
        yield return new WaitForSeconds(0.2f);
        ring1.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(2.15f);
        ring1.GetComponentInParent<Transform>().parent.gameObject.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
