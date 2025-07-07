using UnityEngine;

public class Box : MonoBehaviour
{
    public Transform _parentOfShape;
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entering");
        collision.transform.parent.SetParent(_parentOfShape, false);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.transform.parent.SetParent(_parentOfShape, false);
    }
}
