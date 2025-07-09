using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeDataList;
    public List<Shape> shapeList;


    void Awake()
    {
        GameData.onBoardShapes = new bool[shapeList.Count];
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
            if (!shape.IsOnStartPosition() && shape._isOnDrag)
                return shape;
            Debug.Log($"{shape._isOnDrag}");
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
