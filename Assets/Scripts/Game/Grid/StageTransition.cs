using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageTransition : MonoBehaviour
{
    public static StageTransition instance;

    [SerializeField] private RevertFadeASyncLoading revertFadeASync;
    private StageSwipe stageSwipe;
    private PanelAnimation[] panelAnimation;

    public void ExecuteTransition()
    {
        Debug.Log("it is running");
        StartCoroutine(WaitAndRun());
    }

    private IEnumerator WaitAndRun()
    {
        revertFadeASync = GameObject.FindGameObjectWithTag("Transition").GetComponent<RevertFadeASyncLoading>();
        yield return new WaitForSeconds(4f);
        revertFadeASync.PlayRevertAndLoadDefault();
        yield return new WaitForSeconds(3f);

        if (GameObject.FindGameObjectWithTag("Finish").TryGetComponent(out stageSwipe))
        {
            for (int i = 0; i < stageSwipe.pos.Length; i++) //pos.Length -> transform.childCount
            {
                if (GameData.stageUnlocked[i] == false)
                {
                    stageSwipe.scrollbar.GetComponent<Scrollbar>().value = stageSwipe.pos[i];
                    yield return new WaitForEndOfFrame();
                    Debug.Log(i);
                    break;
                }
            }
            panelAnimation = new PanelAnimation[stageSwipe.pos.Length];
            panelAnimation = GameObject.FindGameObjectWithTag("Finish").GetComponentsInChildren<PanelAnimation>();
            for (int i = 0; i < stageSwipe.pos.Length; i++)
            {
                if (panelAnimation[i] != null && GameData.stageUnlocked[i] == false)
                {
                    Debug.Log("it still work");
                    panelAnimation[i].Running();
                    GameData.stageUnlocked[i] = true;
                    Debug.Log(i);
                    break;
                }
                else Debug.Log("cannot find the component/ set as false");
            }
        }
        else Debug.LogWarning("the code has issues");
    }

    private void Awake()
    {

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
