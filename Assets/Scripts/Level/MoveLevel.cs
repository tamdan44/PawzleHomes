using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveLevel : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] GameObject scrollbar;
    [SerializeField] private int currentPage;
    [SerializeField] private float distance; //distance for the drag to happend
    [SerializeField] private Vector2 targetPos, pageStep;

    [SerializeField] private float[] maxPage;

    private void Move()
    {
        Debug.Log($"wonderful {currentPage}");
        StartCoroutine(MovePage(0.5f));
    }

    private void Next()
    {
        if (currentPage < maxPage.Length)
        {
            currentPage++;
            targetPos += pageStep;
            Move();
        }
        else return;
    }

    private void Previous()
    {
        if (currentPage >= 0)
        {
            currentPage--;
            targetPos -= pageStep;
            Move();
        }
        else return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("this still run");
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > distance)
        {
            Debug.Log("nice");
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else Move();
    }






    void Start()
    {
        maxPage = new float[transform.childCount];
        distance = 1 / (maxPage.Length - 1);
        for (int i = 0; i < maxPage.Length; i++)
        {
            maxPage[i] = distance * i;
        }
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
            float t = Mathf.Lerp(0f, 1f, elapsedTime / moveDuration);
            scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(currentPos, maxPage[currentPage], t);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        scrollbar.GetComponent<Scrollbar>().value = maxPage[currentPage];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
