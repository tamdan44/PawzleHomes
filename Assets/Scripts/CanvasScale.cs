using UnityEngine;
using UnityEngine.UI;

public class CanvasScale : MonoBehaviour
{
    public Canvas canvas;
    public float GetScaleSize()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();

        Vector2 screenSize = new(Screen.width, Screen.height);
        Vector2 refResolution = scaler.referenceResolution;
        float match = scaler.matchWidthOrHeight;

        float width = screenSize.x / refResolution.x;
        float height = screenSize.y / refResolution.y;

        return Mathf.Lerp(width, height, match);
    }
}
