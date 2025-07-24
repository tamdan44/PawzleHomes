using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    private List<ShapeData> shapeDataList = new();
    public List<Shape> shapeList;


    void Awake()
    {
    
        for (int i = 0; i < 25; i++)
        {
            ShapeData shapeData = Resources.Load<ShapeData>($"ShapeDatas/{i}");
            shapeDataList.Add(shapeData);
        }

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
                foreach (GameObject shapeTile in shape._currentTriangles)
                {
                    SaveSystem.ConvertImageColor(shapeTile.GetComponent<ShapeTile>().normalImage, GameData.shapeColor);

                }
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
        }
        Debug.LogError("There is no shape selected!");
        return null;
    }

    public List<int> GetCurrentShapeDatas()
    {
        List<int> shapeDataIndices = new List<int>();
        foreach (var shape in shapeList)
        {
            if(!shape.IsOnStartPosition() && shape.gameObject.activeInHierarchy){
                shapeDataIndices.Add(shape.shapeDataIndex);
                Debug.Log("shape active");
            }
            
        }
        return shapeDataIndices;

    }

    private void ChangeShapeColor(string changedColor)
    {

    }
}
