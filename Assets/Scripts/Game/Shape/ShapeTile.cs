using UnityEngine;
using UnityEngine.UI;
public class ShapeTile : MonoBehaviour
{
    public Image occupiedImage;
    void Start()
    {
        SetUnoccupied();
    }

    public void SetOccupied()
    {
        occupiedImage.gameObject.SetActive(true);
    }

    public void SetUnoccupied()
    {
        occupiedImage.gameObject.SetActive(false);
    }

    public void DeactivateSquare()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }
    public void ActivateSquare()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }
}