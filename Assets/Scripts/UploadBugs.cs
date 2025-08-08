using UnityEngine;
using TMPro;

public class UploadBugs : MonoBehaviour
{
    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private RectTransform grid;
    [SerializeField] private ShapeStorage shapeStorage;
    [SerializeField] private PuzzleBackground bg;
    string errors = GameData.testStage;
    void Start()
    {
        tMP_Text.text = errors;

        if (!grid.gameObject.activeSelf)
        {
            Debug.LogWarning("StagePlayer was disabled — enabling now.");

            errors += "No grid";
        }
        else
            errors += "grid ";
        tMP_Text.text = errors;

        errors += $"{bg.enabled}" + $"x: {grid.localPosition.x} "
        + $"y: {grid.localPosition.y}" + $" {shapeStorage.shapeList[0].transform.localPosition.x} " + $"{shapeStorage.shapeList[0].transform.localPosition.y}";
        tMP_Text.text = errors;


        if (shapeStorage.shapeList.Count > 0)
        {
            errors += $"x: {shapeStorage.shapeList[1].transform.localPosition.x} " + $"y:{shapeStorage.shapeList[1].transform.localPosition.y}";
        }
        else errors += "no_shapeList ";

        tMP_Text.text = errors;

        // if (stagePlayer.isActiveAndEnabled)
        // {
        //     errors += "stagePlayer ";
        // }
        // else errors += "no stagePlayer ";
        // tMP_Text.text = errors;
        try
        {
            Shape shape1 = shapeStorage.shapeList[0].GetComponent<Shape>();
            errors += "getshape ";
            tMP_Text.text = errors;

            if (shape1._currentTriangles.Count > 0)
            {
                errors += $"{shape1._currentTriangles.Count} ";
            }
            
            tMP_Text.text = errors;

        }
        catch
        {
            errors += "getnoshape ";
        }
        ;
        tMP_Text.text = errors;

        // if (!panelAnimation.panelRan)
        // {
        //     if (stagePlayer.panelAnimations.Length > 0)
        //         stagePlayer.panelAnimations[0].InitializeStageUnlocked();   
        // }
        // errors += "panelAnimation.panelRan";
        // tMP_Text.text = errors;

    }

    void Update()
    {
        tMP_Text.text = errors + GameData.testStage;
    }

    void TextErrors(string err, string additional_text)
    {
        err += additional_text;
        tMP_Text.text = err;
    }

}
