using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Transition")]
    [Tooltip("Một Panel full-screen có CanvasGroup, màu đen, phủ lên toàn màn hình để fade")]
    [SerializeField] private CanvasGroup transitionPanel;
    [SerializeField] private float transitionDuration = 0.5f;

    [Header("Slider & Spinner")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private RectTransform spinnerIcon;
    [SerializeField] private float sliderSpeed = 1f;
    [SerializeField] private float minimumLoadTime = 2f;

    [Header("Tips (optional)")]
    [TextArea][SerializeField] private string[] tips;
    [SerializeField] private Text tipText;


    /// <summary>
    /// Được gọi khi user nhấn nút Load
    /// </summary>
    public void LoadLevelBtn(string levelToLoad)
    {
        // Bắt đầu coroutine chuyển cảnh → loading
        StartCoroutine(TransitionThenLoad(levelToLoad));
    }

    /// <summary>
    /// Fade màn hình ra đen, sau đó show loading screen, rồi load async
    /// </summary>
    private IEnumerator TransitionThenLoad(string levelToLoad)
    {
        // 1) Show panel fade và từ từ đưa alpha từ 0 → 1
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.alpha = 0f;
        LeanTween.alphaCanvas(transitionPanel, 1f, transitionDuration)
                 .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(transitionDuration);

        // các code dưới uncommnent nếu không chọn transition panel là loading screen , uncomment để hiểu :))
        // 2) Ẩn main menu, show loading screen
        mainMenu.SetActive(false);
        //loadingScreen.SetActive(true);

        // 3) Hiển thị tip (nếu có)
        if (tipText != null && tips != null && tips.Length > 0)
            StartCoroutine(ShowRandomTip());

        // 4) Fade ngược (từ đen → trong suốt) để lộ loadingScreen
        //LeanTween.alphaCanvas(transitionPanel, 0f, transitionDuration)
        //         .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(transitionDuration);
        // Nếu bạn muốn ẩn transitionPanel sau khi fade xong, uncomment dòng dưới
        //transitionPanel.gameObject.SetActive(false);

        // 5) Bắt đầu load scene bất đồng bộ
        yield return StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator ShowRandomTip()
    {
        string tip = tips[Random.Range(0, tips.Length)];
        tipText.text = "";
        foreach (char c in tip)
        {
            tipText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator LoadLevelAsync(string levelToLoad)
    {
        // Xoay spinner
        LeanTween.rotateAround(spinnerIcon.gameObject, Vector3.forward, -360f, 1f)
                 .setRepeat(-1)
                 .setEase(LeanTweenType.linear);

        var operation = SceneManager.LoadSceneAsync(levelToLoad);
        operation.allowSceneActivation = false;

        float startTime = Time.time;
        float displayedProgress = 0f;

        while (true)
        {
            float rawProgress = Mathf.Clamp01(operation.progress / 0.9f);
            displayedProgress = Mathf.MoveTowards(displayedProgress,
                                                  rawProgress,
                                                  sliderSpeed * Time.deltaTime);
            loadingSlider.value = displayedProgress;

            bool minTimeElapsed = Time.time - startTime >= minimumLoadTime;
            if (operation.progress >= 0.9f &&
                displayedProgress >= 1f &&
                minTimeElapsed)
                break;

            yield return null;
        }

        // Full slider, dừng spinner, kích hoạt chuyển scene
        loadingSlider.value = 1f;
        LeanTween.cancel(spinnerIcon.gameObject);
        operation.allowSceneActivation = true;
    }
}
