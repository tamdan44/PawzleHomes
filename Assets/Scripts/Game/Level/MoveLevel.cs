using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveLevel : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] private int currentPage;
    [SerializeField] private float dragThreshold;

    private float distance;
    public float[] maxPage;

    private void Move()
    {
        StartCoroutine(MovePage(0.2f));
    }

    /*private void Next() //for the Next Button
    {
        if (currentPage < maxPage.Length - 1)
        {
            currentPage++;
            Move();
        }
        else return;
    }

    private void Previous() //for the Button too
    {
        if (currentPage > 0)
        {
            currentPage--;
            Move();
        }
        else return;
    }*/

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
        for (int i = 0; i < maxPage.Length; i++)
        {
            if (scrollbar.value > maxPage[i] - distance / 2 &&
                scrollbar.value < maxPage[i] + distance / 2)
            {
                currentPage = i;
                Move();
            }
        }
        /*if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else Move();*/
    }


    void Start()
    {
        StartCoroutine(WaitForScrollbar());
    }

    private IEnumerator WaitForScrollbar()
    {
        yield return new WaitForSeconds(1f);
        maxPage = new float[transform.childCount];
        distance = 1f / (maxPage.Length - 1f);
        for (int i = 0; i < maxPage.Length; i++)
        {
            maxPage[i] = distance * i;
        }
        scrollbar.value = 0f;
    }

    private IEnumerator MovePage(float moveDuration)
    {
        float currentPos = scrollbar.value;
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / moveDuration);
            scrollbar.value = Mathf.Lerp(currentPos, maxPage[currentPage], t);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        scrollbar.value = maxPage[currentPage];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        scrollRect.OnDrag(eventData);
    }
}
