using UnityEngine;

public class SetPosition : MonoBehaviour
{
    void Awake()
    {
        transform.localPosition = new Vector2(0, 200);
    }

}
