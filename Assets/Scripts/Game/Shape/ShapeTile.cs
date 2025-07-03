using UnityEngine;
using UnityEngine.UI;
public class ShapeTile : MonoBehaviour
{
    void Start()
    {
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