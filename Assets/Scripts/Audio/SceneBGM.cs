using UnityEngine;

public class SceneBGM : MonoBehaviour
{

    [Tooltip("Tên group BGM phải trùng với AudioDatabaseSO")]
    public string bgmGroupName;


    private void Start()
    {

        if (!string.IsNullOrEmpty(bgmGroupName))
            AudioManager.instance.StartBGM(bgmGroupName);

    }
}
