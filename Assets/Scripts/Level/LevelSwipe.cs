using UnityEngine;
using UnityEngine.UI;

public class LevelSwipe : MonoBehaviour
{
    private float distance;
    private float scrollPos = 0;

    private float[] levelmenuPos;
    private Image[] dots;

    [SerializeField] private GameObject scrollHorizontal;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotPlace;
    [SerializeField] private Sprite brownDot, grayDot;

    private void Start()
    {
        Debug.Log(transform.childCount);
        levelmenuPos = new float[transform.childCount];

        dots = new Image[levelmenuPos.Length];

        distance = 1f / (levelmenuPos.Length - 1f);
        for (int i = 0; i < levelmenuPos.Length; i++)
        {
            levelmenuPos[i] = i * distance;
            GameObject dot = Instantiate(dotPrefab, dotPlace);
            dots[i] = dot.GetComponent<Image>();
        }
        //transform.GetComponent<RectTransform>().anchorMax = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollHorizontal.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < levelmenuPos.Length; i++)
            {
                if (scrollPos < levelmenuPos[i] + distance / 2f && scrollPos > levelmenuPos[i] - distance / 2f)
                {
                    scrollHorizontal.GetComponent<Scrollbar>().value =
                        Mathf.Lerp(scrollHorizontal.GetComponent<Scrollbar>().value, levelmenuPos[i], 0.2f);
                    dotPlace.transform.parent.GetComponent<Scrollbar>().value = levelmenuPos[i];
                    UpdateBar(i);
                }
            }
        }
    }
    private void UpdateBar(int j)
    {
        if (j > dots.Length) j = 2;
        dots[j].sprite = grayDot;
        foreach (var item in dots)
        {
            item.transform.localScale = Vector2.one;
            item.sprite = grayDot;
        }
        dots[j].sprite = brownDot;
        dots[j].transform.localScale = Vector2.one * 1.8f;
        
    }
}
