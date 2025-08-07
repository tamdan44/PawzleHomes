using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RevertFadeASyncLoading : MonoBehaviour
{
    [SerializeField] private CanvasGroup cgA;
    [SerializeField] private CanvasGroup cgB;
    [Tooltip("Thời gian cross-fade (giây)")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private string defaultSceneName = "LevelScreen";
    [SerializeField] private float defaultMinimumTime = 1.5f;
    [SerializeField] private bool setDefaultAlphaB = true;
    [SerializeField] private bool setDefaultAlphaA = false;
    private void Awake()
    {
        if (cgA == null || cgB == null)
        {
            Debug.LogError("CanvasGroups cgA and cgB must be assigned.");
            enabled = false;
            return;
        }

        // Thiết lập alpha ban đầu:
        cgA.gameObject.SetActive(setDefaultAlphaA);
        cgB.gameObject.SetActive(setDefaultAlphaB);


    }

    /// <summary>
    /// 1) Revert: fade B→0, A→1
    /// 2) Song song load scene (không kích hoạt ngay)
    /// 3) Chờ cả 3 điều kiện: revert xong, load xong (progress ≥ 0.9), và minTime
    /// 4) Cho phép active scene và disable cgB
    /// </summary>
    /// 

    public void LoadPanel()
    {
        cgA.gameObject.SetActive(true);
        cgB.gameObject.SetActive(true);
    }
    public void PlayRevertAndLoadDefault()
    {
        // Khởi tạo alpha & active
        Debug.Log(cgA.gameObject.activeSelf);
        Debug.Log(cgA.gameObject.GetComponent<CanvasGroup>().name);
        cgA.gameObject.SetActive(true);
        cgB.gameObject.SetActive(true);
        cgA.alpha = 1f;
        cgB.alpha = 0f;
        StartCoroutine(RevertAndLoad(defaultSceneName, defaultMinimumTime));
    }

    private IEnumerator RevertAndLoad(string sceneName, float minimumTime)
    {
        // 1) Reset lại nếu cần (đảm bảo state ban đầu trước revert)
        cgA.gameObject.SetActive(true);
        cgB.gameObject.SetActive(true);
        cgB.alpha = 1f;
        cgA.alpha = 0f;

        // 2) Kick off revert-fade
        LeanTween.alphaCanvas(cgB, 0f, duration).setEaseLinear();
        LeanTween.alphaCanvas(cgA, 1f, duration).setEaseLinear();

        // 3) Bắt đầu load scene async (cho đến 0.9f rồi dừng)
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float startTime = Time.time;
        bool revertDone = false;

        // 4) Chờ cho đến khi cả 3 điều kiện đều true
        while (true)
        {
            // a) kiểm tra revert đã xong chưa
            if (!revertDone && Time.time - startTime >= duration)
                revertDone = true;

            // b) kiểm tra load đã đến 0.9 chưa
            bool loadReady = op.progress >= 0.9f;

            // c) kiểm tra đã đủ minTime chưa
            bool minTimeElapsed = Time.time - startTime >= minimumTime;

            if (revertDone && loadReady && minTimeElapsed)
                break;

            yield return null;
        }

        // 5) Cho phép active scene mới
        op.allowSceneActivation = true;

        // 6) Tắt luôn cgB để dọn dẹp
        cgB.gameObject.SetActive(false);
    }
}

