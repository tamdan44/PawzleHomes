using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveLevel : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] GameObject scrollbar;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] private int currentPage;
    [SerializeField] private float dragThreshold;

    private float distance;
    private float[] maxPage;

    private void Move()
    {
        StartCoroutine(MovePage(0.2f));
    }

    private void Next()
    {
        if (currentPage < maxPage.Length - 1)
        {
            currentPage++;
            Move();
        }
        else return;
    }

    private void Previous()
    {
        if (currentPage > 0)
        {
            currentPage--;
            Move();
        }
        else return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.OnEndDrag(eventData);
        for (int i = 0; i < maxPage.Length; i++)
        {
            if (scrollbar.GetComponent<Scrollbar>().value > maxPage[i] - distance / 2 && 
                scrollbar.GetComponent<Scrollbar>().value < maxPage[i] + distance / 2)
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
        maxPage = new float[transform.childCount];
        distance = 1f / (maxPage.Length - 1f);
        for (int i = 0; i < maxPage.Length; i++)
        {
            maxPage[i] = distance * i;
        }
        scrollbar.GetComponent<Scrollbar>().value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MovePage(float moveDuration)
    {
        float currentPos = scrollbar.GetComponent<Scrollbar>().value;
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / moveDuration);
            scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(currentPos, maxPage[currentPage], t);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        scrollbar.GetComponent<Scrollbar>().value = maxPage[currentPage];
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
