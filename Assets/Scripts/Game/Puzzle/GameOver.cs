using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPopup;
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

    void GameOverPopup(){
        gameOverPopup.SetActive(true);
        petImage.gameObject.SetActive(true);
    }
}
