using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Hint : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GridManager grid;
    [SerializeField] private BuyHint buyHintPanel;
    [SerializeField] private TMP_Text numHint;
    [SerializeField] private TMP_Text noHintMessage;
    [SerializeField] private TMP_Text givingHintMessage;
    [SerializeField] private TMP_Text chooseOtherShapeMessage;
    private Dictionary<int, List<Vector3Int>> currentSolutions;
    private bool _isGivingHint = false;
    private List<int> _hints = new();

    void Start()
    {   
        // noHintMessage.color = Color.white;
        // givingHintMessage.color = Color.white;
        // chooseOtherShapeMessage.color = Color.white;

        currentSolutions = new();
        Debug.Log($"neww hint");

        numHint.text = GameData.numHint.ToString();
        buyHintPanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetHint();
        numHint.text = GameData.numHint.ToString();
        if (_isGivingHint)
            RunTextAppearsThenFades(givingHintMessage, 0.6f);
    }

    public void GetHint()
    {
        if(currentSolutions.Count==0)
            currentSolutions = LoadHint();
        grid.highStar = false;

        if (GameData.numHint > 0 && !_isGivingHint)
        {
            GameData.numHint -= 1;
            _isGivingHint = true;
        }
        else
        {
            RunTextAppearsThenFades(noHintMessage, 0.6f);
            TurnOnBuyHintPanel();
        }
    }
    void TurnOnBuyHintPanel()
    {
        buyHintPanel.gameObject.SetActive(true);
        buyHintPanel.moneyBar.gameObject.SetActive(true);
    }
    void TurnOffBuyHintPanel()
    {
        buyHintPanel.gameObject.SetActive(false);
        buyHintPanel.moneyBar.gameObject.SetActive(false);
    }
    
    public void GiveHintStart(int shapeIndex)
    {
        if (currentSolutions.Count == 0)
            currentSolutions = LoadHint();

        if (_isGivingHint)
        {
            if (_hints.Contains(shapeIndex))
                RunTextAppearsThenFades(chooseOtherShapeMessage, 0.6f);
            else
            {
                _hints.Add(shapeIndex);
                _isGivingHint = false;
            }
        }
        if (_hints.Contains(shapeIndex))
        {
            foreach (var tile in currentSolutions[shapeIndex])
            {
                Debug.Log($"hint{tile[0]}");
                grid.grid[tile.x, tile.y, tile.z].normalImage.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
    public void GiveHintEnd(int shapeIndex)
    {
        foreach (var tile in currentSolutions[shapeIndex])
        {
            grid.grid[tile.x, tile.y, tile.z].normalImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
    
    Dictionary<int, List<Vector3Int>> LoadHint()
    {
        List<Vector3Int> currentSolution;
        Dictionary<int, List<Vector3Int>> currentSolutions = new();
        for (int i = 0; i < GameData.solutions.Count; i++)
        {
            currentSolution = new();
            string[] tiles = GameData.solutions[i].TrimEnd().Split(" ");
            foreach (string tile in tiles)
            {
                string[] t = tile.Split(".");
                currentSolution.Add(new Vector3Int(int.Parse(t[0]), int.Parse(t[1]), int.Parse(t[2])));
            }
            currentSolutions[i] = currentSolution;
        }
            Debug.Log($"currentSolution.Count {currentSolutions[0].Count} {0}");
        return currentSolutions;
    }

    private void RunTextAppearsThenFades(TMP_Text textImg, float waitSeconds)
    {
        StartCoroutine(TextAppearsThenFades(textImg, waitSeconds));
    }


    private IEnumerator TextAppearsThenFades(TMP_Text textImg, float waitSeconds)
    {
        yield return StartCoroutine(Disappear(textImg, 0.5f, 0f, 1));
        yield return new WaitForSeconds(waitSeconds);
        yield return StartCoroutine(Disappear(textImg, 0.5f, 1f, 0));
    }


    private IEnumerator Disappear(TMP_Text _image, float moveDuration, float currentAlpha, float expectedAlpha)
    {
        Debug.Log($"color {_image.color}");
        Color colored = _image.color;
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            float alpha = Mathf.Lerp(currentAlpha, expectedAlpha, elapsedTime / moveDuration);
            colored.a = alpha;
            _image.color = colored;
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveDuration) break;
            yield return null;
        }
        Debug.Log("runnig");
        colored.a = expectedAlpha;
        _image.color = colored;
    }


}
