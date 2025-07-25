using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IDropHandler
{
    public Transform _parentOfShape;

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.SetParent(_parentOfShape, false);
    }
}
