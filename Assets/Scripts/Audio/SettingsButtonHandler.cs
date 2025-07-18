using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingsButtonHandler : MonoBehaviour
{
    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        if (UI_Options.instance != null)
            UI_Options.instance.ShowSettings();
        else
            Debug.LogWarning("UI_Options.instance chưa được khởi tạo!");
    }

    private void OnDestroy()
    {
        // luôn unregister để tránh memory leak
        if (_btn != null)
            _btn.onClick.RemoveListener(OpenSettings);
    }
}
