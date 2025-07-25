using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;

public class RewardedAdPanel : MonoBehaviour
{
    [SerializeField] private loadRewarded RewardedAd;
    [SerializeField] private TMP_Text panelText;
    public bool adWatched { get; set; }

    void Start()
    {
        // loadRewarded RewardedAd = new loadRewarded();
        // gameObject.SetActive(false);
        adWatched = false;
    }
    public void YesClicked()
    {
        RewardedAd.LoadAd();
        adWatched = true;

    }

    public void NoClicked()
    {
        gameObject.SetActive(false);
    }


}
