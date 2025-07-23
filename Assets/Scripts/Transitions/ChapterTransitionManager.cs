using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChapterTransitionManager : MonoBehaviour
{
    [Header("Zoom & Fade Settings")]
    [Tooltip("Tỉ lệ scale sau khi zoom (so với 1)")]
    [SerializeField] private float targetScale = 5f;
    [Tooltip("Thời gian zoom (giây)")]
    [SerializeField] private float zoomDuration = 0.6f;
    [Tooltip("Thời gian fade-out (giây)")]
    [SerializeField] private float fadeDuration = 0.4f;

    /// <summary>
    /// Gọi khi user bấm Play trên một chapter.
    /// </summary>
    /// <param name="card">GameObject của chapter (có RectTransform + CanvasGroup)</param>
    /// <param name="sceneName">Tên scene sẽ load</param>
    public void PlayChapter(GameObject card, string sceneName)
    {
        StartCoroutine(TransitionCoroutine(card, sceneName));
    }

    private IEnumerator TransitionCoroutine(GameObject card, string sceneName)
    {
        // 1) Đem card lên trên cùng
        card.transform.SetAsLastSibling();

        // 2) Cache components
        var rt = card.GetComponent<RectTransform>();
        var cg = card.GetComponent<CanvasGroup>();
        if (rt == null || cg == null)
        {
            Debug.LogError("Chapter card cần RectTransform và CanvasGroup!");
            yield break;
        }

        // 3) Đảm bảo card đang scale=1, alpha=1
        rt.localScale = Vector3.one;
        cg.alpha = 1f;
        card.SetActive(true);

        // 4) Zoom in
        LeanTween.scale(rt, Vector3.one * targetScale, zoomDuration)
                 .setEase(LeanTweenType.easeOutQuad);

        // Chờ zoom xong
        yield return new WaitForSeconds(zoomDuration);

        // 5) Fade out
        LeanTween.alphaCanvas(cg, 0f, fadeDuration)
                 .setEase(LeanTweenType.easeInQuad);

        yield return new WaitForSeconds(fadeDuration);

        // 6) Load scene
        SceneManager.LoadScene(sceneName);
    }
}
