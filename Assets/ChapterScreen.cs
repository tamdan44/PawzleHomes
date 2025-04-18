using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterScreen : MonoBehaviour
{
    public void ReturnToHome()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
