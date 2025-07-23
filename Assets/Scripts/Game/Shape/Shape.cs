using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject shapeImage;
    [SerializeField] private GridManager grid;
    [SerializeField] private float _offsetScale, shapeSelectedScale;
    public int shapeDataIndex, shapeIndex;
    public ShapeData _currentShapeData { get; set; }
    public bool _isActive { get; set; }
    public bool _isOnDrag { get; set; }
    public int TotalTriangleNumber { get; set; }

    public List<GameObject> _currentTriangles = new();
    private RectTransform _transform;
    private Transform _parent;
    private Vector3 _startPosition;
    private Vector3 _shapeStartScale;

    public void Awake()
    {
        _parent = GetComponent<Transform>().parent;
        _transform = GetComponent<RectTransform>();
        _startPosition = _transform.localPosition;
        _shapeStartScale = GetComponent<RectTransform>().localScale * shapeSelectedScale;
        _isActive = false;
        _isOnDrag = false;
    }


    public void SetShapeInactive()
    {
        if (!IsOnStartPosition() && IsAnyOfSquareActive())
            foreach (var square in _currentTriangles)
            {
                square.SetActive(false);
            }
    }

    public bool IsAnyOfSquareActive()
    {
        foreach (var tri in _currentTriangles)
        {
            if (tri.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }
    public void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
        EnterParent();
        MakeShapeVisible();
        GameData.onBoardShapes[shapeIndex] = false;
    }
    
    public void RequestNewShape(ShapeData shapeData)
    {
        _currentShapeData = shapeData;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        TotalTriangleNumber = GetNumberOfTriangle(shapeData);

        while (_currentTriangles.Count < TotalTriangleNumber)
        {
            _currentTriangles.Add(Instantiate(shapeImage, transform) as GameObject);
        }
        foreach (GameObject tri in _currentTriangles)
        {
            tri.transform.position = Vector3.zero;
            tri.SetActive(false);
        }

        var triRect = shapeImage.GetComponent<RectTransform>();
        float side = triRect.rect.width * triRect.localScale.x;
        Vector2 moveDistance = new(MathF.Sqrt(2 * side * side), MathF.Sqrt(2 * side * side));
        int currentIndexInList = 0;
        // _parent = GetComponent<Transform>().parent;

        // set position to form final shapes
        for (int row = 0; row < shapeData.rows; row++)
        {
            for (int column = 0; column < shapeData.columns; column++)
            {
                for (int tri = 0; tri < 4; tri++)
                {

                    if (shapeData.board[row].column[column][tri])
                    {
                        Quaternion rotation = Quaternion.Euler(0, 0, 90 * tri + 135);
                        Vector3 position = tri switch
                        {
                            0 => new Vector3(0, -1), //bot
                            1 => new Vector3(1, 0), //right
                            2 => new Vector3(0, 1), //top
                            3 => new Vector3(-1, 0), //left
                            _ => Vector3.zero
                        };
                        _currentTriangles[currentIndexInList].SetActive(true);
                        _currentTriangles[currentIndexInList].GetComponent<PolygonCollider2D>().enabled = false;
                        _currentTriangles[currentIndexInList].GetComponent<RectTransform>().rotation = rotation;
                        _currentTriangles[currentIndexInList].GetComponent<RectTransform>().localPosition =
                            new Vector3(GetXPositionForShapeSquare(shapeData, column, moveDistance),
                                        GetYPositionForShapeSquare(shapeData, row, moveDistance)) + position * _offsetScale;
                        currentIndexInList++;
                    }
                }
            }
        }
        // Debug.Log($"{_currentTriangles.Count}");
        // if(_currentTriangles.Count>0)
        //     _currentTriangles[0].GetComponent<PolygonCollider2D>().enabled = true;
        _transform.transform.localScale = _shapeStartScale;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (shapeData.columns > 1)
        {
            float startXPos;
            if (shapeData.columns % 2 != 0)
            {
                startXPos = (shapeData.columns / 2) * moveDistance.x;
            }
            else
            {
                startXPos = ((shapeData.columns / 2) - 1) * moveDistance.x + moveDistance.x / 2;
            }
            shiftOnX = startXPos - column * moveDistance.x;
        }
        return shiftOnX;
    }
    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.rows > 1)
        {
            float startYPos;
            if (shapeData.rows % 2 != 0)
            {
                startYPos = (shapeData.rows / 2) * moveDistance.y;
            } else
            {
                startYPos = ((shapeData.rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            }
            shiftOnY = startYPos - row * moveDistance.y;
        }
        return shiftOnY;
    }

    private int GetNumberOfTriangle(ShapeData shapeData)
    {
        int number = 0;
        foreach (bool active in shapeData.triangles)
        {
            if (active)
            {
                number++; // number of 'true' triangle                                 
            }
        }
        return number;
    }

    public void MakeShapeVisible()
    {
        foreach (var tri in _currentTriangles)
        {
            tri.GetComponent<ShapeTile>().MakeTileVisible();
        }
    }
    public void MakeShapeInvisible()
    {
        foreach (var tri in _currentTriangles)
        {
            tri.GetComponent<ShapeTile>().MakeTileInvisible();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _transform.localScale = _shapeStartScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0.5f, 0.5f);
        _transform.anchorMax = new Vector2(0.5f, 0.5f);
        _transform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isOnDrag = true;
        MakeShapeVisible();
        if (SceneManager.GetActiveScene().name != "Puzzle") grid.GiveHintStart(shapeIndex);

        if (GameData.onBoardShapes[shapeIndex]) //if it's already onboard, remove the visible
        {
            foreach (var square in grid.grid)
            {
                GridTile gridTile = square.GetComponent<GridTile>();
                if (gridTile.collisionShapeIndices.Contains(shapeIndex))
                {
                    gridTile.SwitchShapeVisibility();
                    gridTile.collisionShapeIndices.Remove(shapeIndex);
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        ExitParent();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_transform,
            eventData.position, Camera.main, out Vector3 pos);
        _transform.position = pos;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name != "Puzzle") grid.GiveHintEnd(shapeIndex);

        GameEvents.CheckIfShapeCanBePlaced();
        _transform.localScale = _shapeStartScale;
        _isOnDrag = false;
    }
    public void EnterParent()
    {
        transform.SetParent(_parent, false);
    }
    public void ExitParent()
    {
        var greatgrandparent = _parent.parent.parent;   //the ShapeHolder
        transform.SetParent(greatgrandparent, true);
        transform.SetAsFirstSibling();
    }
}