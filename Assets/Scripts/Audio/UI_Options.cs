using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    public static UI_Options instance;

    [Header("Mixer (drag-in)")]
    public AudioMixer audioMixer;

    [Header("BGM Settings")]
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParameter = "BGM_KEY";
    private bool currentBGM;

    [Header("SFX Settings")]
    [SerializeField] private Toggle sfxToggle;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParameter = "SFX_KEY";
    private bool currentSFX;

    private void Awake()
    {
        // ––– Singleton –––
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ––– Null-checks –––
        if (audioMixer == null) Debug.LogError("UI_Options: AudioMixer not assigned!");
        if (bgmToggle == null || sfxToggle == null) Debug.LogError("UI_Options: Toggles not assigned!");

        // ––– Đăng ký event –––
        bgmSlider.value = 1;
        sfxSlider.value = 1;

        bgmToggle.onValueChanged.AddListener(SetBGMEnabled);
        sfxToggle.onValueChanged.AddListener(SetSFXEnabled);

        bgmSlider.onValueChanged.AddListener(BGMVolume);
        sfxSlider.onValueChanged.AddListener(SFXVolume);

        BGMVolume(bgmSlider.value);
        SFXVolume(sfxSlider.value);
    }

    private void BGMVolume(float dB)
    {
        float decibel = Mathf.Log10(Mathf.Clamp(dB, 0.00001f, 1f)) * 20f;
        bgmSlider.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = dB.ToString("0.00");
        if (audioMixer != null)
        {
            audioMixer.SetFloat(bgmParameter, decibel);
        }
    }
    private void SFXVolume(float dB)
    {
        float decibel = Mathf.Log10(Mathf.Clamp(dB, 0.00001f, 1f)) * 20f;
        sfxSlider.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = dB.ToString("0.00");
        if (audioMixer != null)
        {
            audioMixer.SetFloat(sfxParameter, decibel);
        }
    }

    private void Start()
    {
        // 1) Bắt buộc load prefs ngay khi scene bắt đầu
        LoadSettings();

        // 2) Rồi ẩn panel cho đến khi user bấm Settings
        gameObject.SetActive(false);
    }

    public void LoadSettings()
    {
        // Đọc prefs (default = on)
        bool bgmOn = PlayerPrefs.GetInt(bgmParameter, 1) == 1;
        bool sfxOn = PlayerPrefs.GetInt(sfxParameter, 1) == 1;

        // Update UI mà không trigger listener
        bgmToggle.SetIsOnWithoutNotify(bgmOn);
        sfxToggle.SetIsOnWithoutNotify(sfxOn);

        // Áp dụng vào mixer và ghi lại state
        SetBGMEnabled(bgmOn);
        SetSFXEnabled(sfxOn);

        Debug.Log($"[UI_Options] Loaded BGM:{bgmOn} SFX:{sfxOn}");
    }

    public void SetBGMEnabled(bool on)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(bgmParameter, on ? 0f : -80f);
            bgmSlider.value = on ? 1f : 0f;
        }
        currentBGM = on;
    }
    public void SetSFXEnabled(bool on)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(sfxParameter, on ? 0f : -80f);
            sfxSlider.value = on ? 1f : 0f;
        }
        currentSFX = on;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt(bgmParameter, currentBGM ? 1 : 0);
        PlayerPrefs.SetInt(sfxParameter, currentSFX ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"[UI_Options] Saved BGM:{currentBGM} SFX:{currentSFX}");
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey(bgmParameter);
        PlayerPrefs.DeleteKey(sfxParameter);
        LoadSettings();
        Debug.Log("[UI_Options] Reset to defaults.");
    }

    public void ShowSettings() => gameObject.SetActive(true);
    public void HideSettings() => gameObject.SetActive(false);

    private void OnDestroy()
    {
        // Unregister để tránh memory-leak
        bgmToggle.onValueChanged.RemoveListener(SetBGMEnabled);
        sfxToggle.onValueChanged.RemoveListener(SetSFXEnabled);

        bgmSlider.onValueChanged.RemoveListener(BGMVolume);
        sfxSlider.onValueChanged.RemoveListener(SFXVolume);
    }
}
