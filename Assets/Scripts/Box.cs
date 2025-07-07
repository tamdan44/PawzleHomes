using UnityEngine;

public class Box : MonoBehaviour
{
    public Transform _parentOfShape;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entering");
        collision.transform.parent.SetParent(_parentOfShape, false);
    }
}
