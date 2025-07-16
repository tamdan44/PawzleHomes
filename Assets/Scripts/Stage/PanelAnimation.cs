using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    public GameObject locker;
    public void InitializeStageUnlocked()
    {
        transform.gameObject.SetActive(false);
    }
    public void Running()
    {
        StartCoroutine(Execute());
    }
    private IEnumerator Execute()
    {
        GetComponentInParent<ScrollRect>().enabled = false;
        gameObject.GetComponentInChildren<AudioSource>().Play();
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(Jiggle(locker.transform, 0.2f, 5f));
        yield return new WaitForSeconds(0.8f);
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.7f);
        yield return StartCoroutine(Move(locker.transform, Vector2.up, 0.3f));
        StartCoroutine(Disappear(transform, 5f));
        StartCoroutine(Disappear(locker.transform, 1f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Move(locker.transform, Vector2.down * 5f, 2f));
        GetComponentInParent<ScrollRect>().enabled = true;
    }


    private IEnumerator Move(Transform _transform, Vector2 endPos, float moveDuration)
    {
        Vector2 startPos = _transform.position;
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            float t = Mathf.Lerp(0f, 1f, Mathf.Clamp01(elapsedTime / moveDuration));
            _transform.position = Vector2.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        _transform.position = endPos;
    }
    private IEnumerator Disappear(Transform _transform, float moveDuration)
    {
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            _transform.GetComponent<Image>().canvasRenderer.SetAlpha(Mathf.Lerp(1f, 0f, Mathf.Clamp01(elapsedTime / moveDuration)));
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        _transform.GetComponent<Image>().canvasRenderer.SetAlpha(0f);
    }
    private IEnumerator Jiggle(Transform _transform, float moveDuration, float totalDuration)
    {
        Vector3 startPos = _transform.position;
        float totalCycle = 0;
        while (totalCycle < totalDuration)
        {
            float offset_X = UnityEngine.Random.Range(-10f, 10f);
            float offset_Y = UnityEngine.Random.Range(-10f, 10f);
            Vector3 newPos = new(_transform.localPosition.x - offset_X, _transform.localPosition.y - offset_Y);
            Debug.Log(newPos);
            float elapsedTime = 0;
            while (elapsedTime < moveDuration)
            {
                float t = Mathf.Lerp(0f, 1f, Mathf.Clamp01(elapsedTime / moveDuration));
                _transform.localPosition = Vector2.Lerp(startPos, newPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
                if (elapsedTime >= moveDuration || _transform.localPosition == newPos) break;
            }
            _transform.localPosition = newPos;

            float returnTime = 0;
            while (returnTime < moveDuration)
            {
                float z = Mathf.Lerp(0f, 1f, Mathf.Clamp01(returnTime / moveDuration));
                _transform.localPosition = Vector2.Lerp(newPos, startPos, z);
                returnTime += Time.deltaTime;
                if (returnTime >= moveDuration || _transform.localPosition == startPos) break;
                yield return null;
            }
            _transform.localPosition = startPos;
            totalCycle ++;
            if (totalCycle >= totalDuration) break;
            yield return null;
        }
    }

}
