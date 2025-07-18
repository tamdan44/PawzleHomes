using UnityEngine;
using UnityEngine.UI;

public class StageSwipe : MonoBehaviour
{
    public GameObject scrollbar;
    // public bool[] stageUnlocked;
    public int currentStateIndex;

    private float scroll_pos = 0;
    private float[] pos;
    private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //GameData.stageUnlocked = new bool[transform.childCount];

        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);
        for (int i = 1; i < pos.Length; i++)
        {
            pos[i] = distance * i;
            //GameData.stageUnlocked[i] = false;
        }

        GameData.stageUnlocked[0] = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + distance / 2 && scroll_pos > pos[i] - distance / 2)
                {
                    scrollbar.GetComponent<Scrollbar>().value =
                        Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.05f);
                    currentStateIndex = i;
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + distance / 2 && scroll_pos > pos[i] - distance / 2)
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, Vector2.one * 1.3f, 0.2f);
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, Vector2.one, 0.1f);
                    }
                }
            }
        }
    }

}
