using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [Header("Loading UI")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;  // thời gian fade in/out
    [SerializeField] private GameObject loadingScreen;     // Panel chứa Slider và Text
    [SerializeField] private Slider progressBar;           // Kéo Slider từ Inspector
    [SerializeField] private Text progressText;            // (Tuỳ chọn)

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Public API để load scene sync (nếu bạn không cần loading screen)
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Public API để load scene async, kèm loading screen
    /// </summary>
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(DoLoadSceneAsync(sceneName));
    }

    private IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        cg.alpha = from;
        // Khi cg.blocksRaycasts = true thì UI sẽ block mọi click bên dưới
        cg.blocksRaycasts = true;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;         // dùng unscaled để không ảnh hưởng timeScale
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
        // Nếu đã fade-out (to=0), bạn có thể cho phép raycast xuyên qua
        cg.blocksRaycasts = to > 0;
    }

    private IEnumerator DoLoadSceneAsync(string sceneName)
    {
        // 1) Bật UI trước (nhưng alpha=0)
        loadingScreen.SetActive(true);

        // 2) Fade in (0 → 1)
        yield return StartCoroutine(Fade(loadingCanvasGroup, 0f, 1f, fadeDuration));

        // 3) Bắt đầu load Async nhưng chưa activate
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        float minDisplayTime = 0.8f;
        float startTime = Time.time;

        // 4) Đợi load xong (0 → 0.9) và đủ thời gian tối thiểu
        while (op.progress < 0.9f || Time.time - startTime < minDisplayTime)
        {
            float prog = Mathf.Clamp01(op.progress / 0.9f);
            if (progressBar) progressBar.value = prog;
            if (progressText) progressText.text = $"{Mathf.RoundToInt(prog * 100)}%";
            yield return null;
        }

        // 5) Active scene mới
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        // 6) Fade out (1 → 0)
        yield return StartCoroutine(Fade(loadingCanvasGroup, 1f, 0f, fadeDuration));

        // 7) Ẩn toàn bộ loadingScreen
        loadingScreen.SetActive(false);
    }


}
