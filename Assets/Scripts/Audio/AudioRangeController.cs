using UnityEngine;

public class AudioRangeController : MonoBehaviour
{
    private AudioSource source;
    private Transform something;

    [SerializeField] private float minDistanceToHearSound = 12;
    [SerializeField] private bool showGizmo;
    private float maxVolume;

    private void Start()
    {
        //something = Camera.instance.transform;
        source = GetComponent<AudioSource>();

        maxVolume = source.volume;
    }

    private void Update()
    {
        if (something == null)
            return;

        float distance = Vector2.Distance(something.position, transform.position);
        float t = Mathf.Clamp01(1 - (distance / minDistanceToHearSound));

        float targetVolume = Mathf.Lerp(0, maxVolume, t * t);
        source.volume = Mathf.Lerp(source.volume, targetVolume, Time.deltaTime * 3);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, minDistanceToHearSound);
        }
    }
}
