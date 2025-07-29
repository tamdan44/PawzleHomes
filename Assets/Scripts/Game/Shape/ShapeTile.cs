using UnityEngine;
using UnityEngine.UI;
public class ShapeTile : MonoBehaviour
{
    public Image normalImage;
    void Start()
    {
        // this.GetComponent<PolygonCollider2D>().enabled = false;
    }

    public void MakeTileVisible()
    {
        normalImage.gameObject.SetActive(true);
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void MakeTileInvisible()
    {
        normalImage.gameObject.SetActive(false);
        GetComponent<PolygonCollider2D>().enabled = false;
    }

}