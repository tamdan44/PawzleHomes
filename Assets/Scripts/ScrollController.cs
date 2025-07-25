using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private int maxPage;
    [SerializeField] private int currentPage = 0;
    Vector3 targetPos;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform levelPagesRect;

    [SerializeField] private float tweenTime;
    [SerializeField] private LeanTweenType tweenType;

    [SerializeField] private float dragThreshold = Screen.width / 15;

    private void Awake()
    {
        targetPos = levelPagesRect.localPosition;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void OnPrevious()
    {
        if (currentPage > 0)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x < eventData.pressPosition.x) Next();
            else OnPrevious();
        }
        else MovePage();
    }
}