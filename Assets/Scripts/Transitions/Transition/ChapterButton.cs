using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChapterButton : MonoBehaviour
{
    [Tooltip("GameObject của chapter card tương ứng")]
    [SerializeField] private GameObject chapterCard;
    [Tooltip("Tên scene sẽ load")]
    [SerializeField] private string sceneToLoad;
    [Tooltip("Drag thẳng GameObject chứa ChapterTransitionManager vào đây")]
    [SerializeField] private ChapterTransitionManager transitionMgr;

    private void Awake()
    {
        chapterCard.SetActive(true);
        GetComponent<Button>().onClick.AddListener(OnClick);
        chapterCard.SetActive(false);
    }

    private void OnClick()
    {
        chapterCard.GetComponent<RectTransform>().localScale = Vector3.one; // Đảm bảo card đang scale=1
        chapterCard.GetComponent<CanvasGroup>().alpha = 1f; // Đảm bảo card đang alpha=1
        transitionMgr.PlayChapter(chapterCard, sceneToLoad);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(OnClick);
        chapterCard.SetActive(false); // Ẩn card khi nút bị hủy
    }
}
