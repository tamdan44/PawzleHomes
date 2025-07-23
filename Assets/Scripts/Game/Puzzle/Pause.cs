using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject popup;
    public Image petImage;

    void Start()
    {
        popup.SetActive(false);
    }

    public void PausePopup()
    {
        popup.SetActive(true);
        petImage.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }
}
