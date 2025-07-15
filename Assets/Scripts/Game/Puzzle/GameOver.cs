using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject popup;
    public Starr star1;
    public Starr star2;
    public Image petImage;

    void Start()
    {
        popup.SetActive(false);
    }

    public void GameOverPopup(int numStars)
    {
        if (numStars == 1)
        {
            star1.SetStarActive();
        }
        if (numStars == 2)
        {
            star1.SetStarActive();
            star2.SetStarActive();
        }
        popup.SetActive(true);
        petImage.gameObject.SetActive(true);
    }


}
