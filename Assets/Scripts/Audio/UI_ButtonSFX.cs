using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSFX : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Tên SFX phải khớp với group trong AudioDatabaseSO")]
    public string clickSfxName = "button-click";

    private Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        // Nếu bạn chỉ muốn nghe onClick (không dùng IPointerClickHandler)
        // thì có thể dùng:
        // _btn.onClick.AddListener(PlayClickSfx);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSfx();
    }

    private void PlayClickSfx()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayGlobalSFX(clickSfxName);
        else
            Debug.LogWarning("No AudioManager.instance found.");
    }

    void OnDestroy()
    {
        // Nếu bạn dùng onClick.AddListener thì nhớ RemoveListener ở đây
        // _btn.onClick.RemoveListener(PlayClickSfx);
    }
}
