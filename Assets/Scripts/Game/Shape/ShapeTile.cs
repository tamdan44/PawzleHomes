using UnityEngine;
using UnityEngine.UI;
public class ShapeTile : MonoBehaviour
{
    public Image normalImage;
    void Start()
    {
    }

    public void MakeTileVisible()
    {
        normalImage.gameObject.SetActive(true);
        this.GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void MakeTileInvisible()
    {
        normalImage.gameObject.SetActive(false);
        this.GetComponent<PolygonCollider2D>().enabled = false;
    }
        
    // public void DeactivateSquare()
    // {
    //     gameObject.GetComponent<BoxCollider2D>().enabled = false;
    //     gameObject.SetActive(false);
    // }
    // public void ActivateSquare()
    // {
    //     gameObject.GetComponent<BoxCollider2D>().enabled = true;
    //     gameObject.SetActive(true);
    // }
}