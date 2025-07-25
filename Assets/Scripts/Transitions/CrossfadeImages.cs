using System.Collections;
using UnityEngine;
public class CrossfadeImages : MonoBehaviour
{
    public static CrossfadeImages instance;

    [SerializeField] private CanvasGroup cgA;
    [SerializeField] private CanvasGroup cgB;
    [Tooltip("Thời gian chuyển cảnh (giây)")]
    [SerializeField] private float duration = 1f;

    private void Awake()
    {
        if (cgA == null || cgB == null)
        {
            Debug.LogError("CanvasGroups cgA and cgB must be assigned.");
            enabled = false;
            return;
        }
        // Thiết lập alpha ban đầu:
        cgA.alpha = 1f;
        cgB.alpha = 0f;
        // Giữ nguyên trạng thái active để tween có hiệu lực
        cgA.gameObject.SetActive(true);
        cgB.gameObject.SetActive(true);

    }

    private void Start()
    {
        StartCoroutine(DoCrossfade());
    }
    public void StartCrossfade()
    {
        if (cgA == null || cgB == null)
        {
            Debug.LogError("CanvasGroups cgA and cgB must be assigned before starting crossfade.");
            return;
        }

        // 1) Bật lại cả hai object
        cgA.gameObject.SetActive(true);
        cgB.gameObject.SetActive(true);

        // 2) Reset alpha về trạng thái ban đầu
        cgA.alpha = 1f;
        cgB.alpha = 0f;

        // 3) Bắt đầu tween
        StartCoroutine(DoCrossfade());
    }


    public IEnumerator DoCrossfade()
    {
        // 1. Fade A đi
        LeanTween.alphaCanvas(cgA, 0f, duration)
                .setEaseLinear();
        // 2. Fade B lên
        LeanTween.alphaCanvas(cgB, 1f, duration)
                .setEaseLinear();

        if (!AudioManager.instance)
        {
            Debug.LogWarning("AudioManager instance is not found. Please ensure AudioManager is present in the scene.");
        }
        else
        {
            AudioManager.instance.PlayGlobalSFX("pop-up");
        }

        // 3. Chờ animation hoàn tất
        yield return new WaitForSeconds(duration);


        cgA.gameObject.SetActive(false);
    }
    public IEnumerator DoFadeA(float targetAlpha, float duration)
    {
        LeanTween.alphaCanvas(cgA, targetAlpha, duration)
                .setEaseLinear();
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator DoFadeB(float targetAlpha, float duration)
    {
        LeanTween.alphaCanvas(cgB, targetAlpha, duration)
                .setEaseLinear();
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator DoRevertFade()
    {
        // 1. Fade B đi
        LeanTween.alphaCanvas(cgB, 0f, duration)
                .setEaseLinear();
        // 2. Fade A lên
        LeanTween.alphaCanvas(cgA, 1f, duration)
                .setEaseLinear();
        // 3. Chờ animation hoàn tất
        yield return new WaitForSeconds(duration);
    }
    public void SetCanvasGroupAActive(bool isActive)
    {
        if (cgA != null)
        {
            cgA.gameObject.SetActive(isActive);
        }
    }
    public void SetCanvasGroupBActive(bool isActive)
    {
        if (cgB != null)
        {
            cgB.gameObject.SetActive(isActive);
        }
    }
    public void SetCanvasGroupAAlpha(float alpha)
    {
        if (cgA != null)
        {
            cgA.alpha = alpha;
        }
    }
    public void SetCanvasGroupBAlpha(float alpha)
    {
        if (cgB != null)
        {
            cgB.alpha = alpha;
        }
    }

    public void StartDoRevertFade()
    {
        if (cgA == null || cgB == null)
        {
            Debug.LogError("CanvasGroups cgA and cgB must be assigned before reverting fade.");
            return;
        }
        cgB.gameObject.SetActive(true); // Đảm bảo cgB được kích hoạt trước khi bắt đầu fade
        cgA.gameObject.SetActive(true); // Đảm bảo cgA được kích hoạt trước khi bắt đầu fade

        // Bắt đầu coroutine để thực hiện revert fade
        StartCoroutine(DoRevertFade());
    }
}
