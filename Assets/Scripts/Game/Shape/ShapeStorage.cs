using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeDataList;
    public List<Shape> shapeList;
    public int shapeDataIndex { get; set; }

    void Start()
    {

    }

    void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }

    public void RequestNewShapes()
    {
        foreach (Shape shape in shapeList)
        {
            if (shape._isActive)
            {
                shape.RequestNewShape(shapeDataList[shape.shapeDataIndex]);
                shape.MoveShapeToStartPosition();
            }
        }
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            if (!shape.IsOnStartPosition() && shape.IsAnyOfSquareActive())
                return shape;
        }

        Debug.LogError("There is no shape selected!");
        return null;
    }

    public List<int> GetCurrentShapeDatas()
    {
        List<int> shapeDataIndices = new List<int>();
        foreach (var shape in shapeList)
        {
            if(shape.gameObject.activeSelf){
                shapeDataIndices.Add(shape.shapeDataIndex);
                Debug.Log("shape active");
            }
            
        }
        return shapeDataIndices;

    }
}
