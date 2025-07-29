using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageTransition : MonoBehaviour
{
    public static StageTransition instance;

    [SerializeField] private RevertFadeASyncLoading revertFadeASync;
    // private StageSwipe stageSwipe;
    // private PanelAnimation[] panelAnimation;

    public void ExecuteTransition()
    {
        Debug.Log("it is running");
        StartCoroutine(WaitAndRun());
    }

    private IEnumerator WaitAndRun()    
    {
        yield return new WaitForEndOfFrame();
        revertFadeASync = GameObject.FindGameObjectWithTag("Transition").GetComponent<RevertFadeASyncLoading>();
        yield return new WaitForSeconds(1.75f);
        AudioManager.instance.PlayGlobalSFX("level-unlock");
        yield return new WaitForSeconds(1f);
        revertFadeASync.PlayRevertAndLoadDefault();
        yield return new WaitForSeconds(3f);

        // if (GameObject.FindGameObjectWithTag("Finish").TryGetComponent(out stageSwipe))
        // {
        //     for (int i = 0; i < stageSwipe.pos.Length; i++) //pos.Length -> transform.childCount
        //     {
        //         if (GameData.stageUnlocked[i] == false)
        //         {
        //             stageSwipe.scrollbar.GetComponent<Scrollbar>().value = stageSwipe.pos[i];
        //             yield return new WaitForEndOfFrame();
        //             panelAnimation = new PanelAnimation[stageSwipe.pos.Length];
        //             panelAnimation = GameObject.FindGameObjectWithTag("Finish").GetComponentsInChildren<PanelAnimation>();
        //             panelAnimation[i].Running();
        //             Debug.Log(panelAnimation[i].name);
        //             GameData.stageUnlocked[i] = true;
        //             /*for (int j = 0; j < GameData.stageUnlocked.Length; j++)
        //             {
        //                 Debug.Log($"{GameData.stageUnlocked[j]} + {panelAnimation[j].name} _> stageTransition");
        //             }*/
        //             break;
        //         }
        //     }
        // }
        // else Debug.LogWarning("the code has issues");
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
