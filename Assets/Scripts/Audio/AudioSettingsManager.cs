using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{


    [Header("Mixer")]
    [Tooltip("Kéo thả AudioMixer của bạn vào đây")]
    public AudioMixer audioMixer;

    [Header("UI Toggles")]
    public Toggle sfxToggle;
    public Toggle bgmToggle;


    [SerializeField] private string sfxParameter = "SFX_KEY";
    [SerializeField] private string bgmParameter = "BGM_KEY";

    private void Start()
    {
        // Đảm bảo AudioMixer đã được thiết lập
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer is not assigned in AudioSettingsManager.");
            return;
        }

        LoadSettings();
    }


    #region Thiết lập volume
    public void SetSFXEnabled(bool on)
    {

        audioMixer.SetFloat(sfxParameter, on ? 0f : -80f);
        PlayerPrefs.SetInt(sfxParameter, on ? 1 : 0);
    }

    public void SetBGMEnabled(bool on)
    {
        audioMixer.SetFloat(bgmParameter, on ? 0f : -80f);
        PlayerPrefs.SetInt(bgmParameter, on ? 1 : 0);
    }

    #endregion

    /// <summary>
    /// Gọi phương thức này từ nút SAVE
    /// </summary>
    public void SaveSettings()
    {

        PlayerPrefs.Save();
        Debug.Log("Audio settings saved.");
        Debug.Log($"SFX: {sfxToggle.isOn}, BGM: {bgmToggle.isOn}");
    }

    /// <summary>
    /// Đọc lại trạng thái từ PlayerPrefs và cập nhật toggle + mixer
    /// </summary>
    private void LoadSettings()
    {
        bool sfxOn = PlayerPrefs.GetInt(sfxParameter, 1) == 1;
        bool bgmOn = PlayerPrefs.GetInt(bgmParameter, 1) == 1;
        //bool ctrlOn = PlayerPrefs.GetInt(CTRL_KEY, 1) == 1;

        // Gán toggle trước để gọi listener và set mixer
        bgmToggle.isOn = bgmOn;
        sfxToggle.isOn = sfxOn;
        SetSFXEnabled(sfxOn);
        SetBGMEnabled(bgmOn);
        //controlToggle.isOn = ctrlOn;
    }

}
