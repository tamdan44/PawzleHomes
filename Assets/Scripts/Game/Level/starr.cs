using UnityEngine;
using UnityEngine.UI;

public class Starr : MonoBehaviour
{
    public Image grayStar;
    public Image activeStar;

    public void SetStarActive()
    {
        grayStar.gameObject.SetActive(false);
        activeStar.gameObject.SetActive(true);
    }

    public void SetStarInactive()
    {
        activeStar.gameObject.SetActive(false);
        grayStar.gameObject.SetActive(true);
    }

    public bool IsActive()
    {
        return activeStar.gameObject.activeSelf;
    }
}
