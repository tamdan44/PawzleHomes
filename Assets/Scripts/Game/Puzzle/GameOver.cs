using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPopup;
    public Starr star1;
    public Starr star2;
    public Image petImage;

    void Start()
    {
        gameOverPopup.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.GameOver += GameOverPopup;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= GameOverPopup;
    }

    void GameOverPopup(int numStars){
        // if (numStars == 1)
        // {
        //     star1.SetStarActive();
        // } if (numStars == 2)
        // {
        //     star1.SetStarActive();
        //     star2.SetStarActive();
        // }
        gameOverPopup.SetActive(true);
        petImage.gameObject.SetActive(true);
    }
}
