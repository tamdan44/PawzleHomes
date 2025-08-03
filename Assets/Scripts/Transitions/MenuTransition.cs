using System.Collections;
using UnityEngine;

public class MenuTransition : MonoBehaviour
{
    [Header("Portal")]
    [Tooltip("Full-screen mask for portal effect using custom shader")]
    public RectTransform portalMask;

    [Header("Cat Fly-In")]
    [Tooltip("RectTransform of the cat sprite, starts off-screen")]
    public RectTransform catSprite;

    [Header("Neon Frame")]
    [Tooltip("CanvasGroup for the neon Pawzle Home frame")]
    public CanvasGroup neonFrame;

    [Header("Menu Group")]
    [Tooltip("CanvasGroup containing Play/Supply/Pawzie buttons")]
    public CanvasGroup menuGroup;

    [Header("Timings")]
    public float portalTime = 0.6f;
    public float catTime = 0.7f;
    public float neonTime = 0.5f;
    public float menuFadeTime = 0.3f;

    private void Start()
    {
        // Initialize states
        gameObject.SetActive(true);
        portalMask.gameObject.SetActive(false);

        catSprite.anchoredPosition = new Vector2(-Screen.width, 0);
        catSprite.gameObject.SetActive(false);

        neonFrame.alpha = 0f;

        menuGroup.alpha = 0f;
        menuGroup.interactable = false;
        menuGroup.blocksRaycasts = false;
        menuGroup.gameObject.SetActive(false);

        // Begin transition sequence
        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        yield return null;
        // 1) Portal expand via custom shader Cutoff
        //portalMask.gameObject.SetActive(true);
        //var portalImage = portalMask.GetComponent<Image>();
        //Material mat = portalImage.material;
        //mat.SetFloat("_Cutoff", 0f);
        //LeanTween.value(gameObject, 0f, 1f, portalTime)
        //         .setEase(LeanTweenType.easeOutQuad)
        //         .setOnUpdate(v => mat.SetFloat("_Cutoff", v));
        //yield return new WaitForSeconds(portalTime);

        portalMask.gameObject.SetActive(true);
        // khởi tạo fill = 0
        var img = portalMask.GetComponent<UnityEngine.UI.Image>();
        img.fillAmount = 0f;
        // tweener tăng dần fillAmount
        LeanTween.value(portalMask.gameObject, 0f, 1f, portalTime)
                 .setEase(LeanTweenType.easeOutQuad)
                 .setOnUpdate((float v) => { img.fillAmount = v; });
        yield return new WaitForSeconds(portalTime);

        // 2) Cat fly-in to center
        catSprite.gameObject.SetActive(true);
        LeanTween.moveLocal(catSprite.gameObject, Vector3.zero, catTime)
                 .setEase(LeanTweenType.easeOutBack);
        yield return new WaitForSeconds(catTime);

        // 3) Neon frame fade-in
        LeanTween.alphaCanvas(neonFrame, 1f, neonTime)
                 .setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(neonTime);

        // 4) Menu buttons fade and enable interaction
        menuGroup.gameObject.SetActive(true);
        LeanTween.alphaCanvas(menuGroup, 1f, menuFadeTime)
                 .setOnComplete(() =>
                 {
                     menuGroup.interactable = true;
                     menuGroup.blocksRaycasts = true;
                 });

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
