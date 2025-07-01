using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GridManager : MonoBehaviour
{
    [SerializeField] private ShapeStorage shapeStorage;
    [SerializeField] private int _width, _height;
    [SerializeField] private GridTile _tilePrefab;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _gridTileScale;
    [SerializeField] private float _positionOffsetScale;
    [SerializeField] private Vector2 startPosition = new Vector2(0.0f, 0.0f);
    [SerializeField] float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    public GridTile[,,] grid;

    void Start()
    {
        grid = new GridTile[_width, _height, 4];
        SpawnGridTiles();
        SpawnLevel();

    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.ClearGrid += ClearGridAndSpawnShapes;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.ClearGrid -= ClearGridAndSpawnShapes;
    }

    void ClearGridAndSpawnShapes()
    {
        foreach (var square in grid)
        {
            square.GetComponent<GridTile>().isVisible = false;
            square.GetComponent<GridTile>().visibleImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < GameData.shapeDataIndices.Count; i++)
        {
            shapeStorage.shapeList[i]._isActive = true;
            shapeStorage.shapeList[i].shapeIndex = GameData.shapeDataIndices[i];
            GameEvents.RequestNewShapes();
        }
    }

    void SpawnGridTiles()
    {
        for (int t = 0; t < 4; t++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 90 * t + 135);
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 position_offset = t switch
                    {
                        0 => new Vector3(0, -0.3334f, 0), //bot
                        1 => new Vector3(0.3334f, 0, 0), //right
                        2 => new Vector3(0, 0.3334f, 0), //top
                        3 => new Vector3(-0.3334f, 0, 0), //left
                        _ => new Vector3(0, 0, 0),
                    };

                    grid[x, y, t] = Instantiate(_tilePrefab);
                    grid[x, y, t].transform.SetParent(this.transform);
                    grid[x, y, t].transform.localRotation = rotation;
                    grid[x, y, t].transform.localScale = grid[x, y, t].transform.localScale * _gridTileScale;
                    grid[x, y, t].TileIndex = new Vector3Int(x, y, t);

                    var tri_rect = grid[x, y, t].GetComponent<RectTransform>();
                    _offset.x = tri_rect.rect.width * tri_rect.transform.localScale.x + everySquareOffset;
                    _offset.y = tri_rect.rect.width * tri_rect.transform.localScale.x + everySquareOffset;

                    Vector3 pos_offset = new Vector3(startPosition.x + _offset.x * x, startPosition.y - _offset.y * y, 0) + position_offset * _positionOffsetScale;
                    grid[x, y, t].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos_offset.x, pos_offset.y);
                    grid[x, y, t].GetComponent<RectTransform>().localPosition = pos_offset;

                }
            }
        }
    }
    //check if shape can be placed, if it can, place it on the grid, check and add scores
    private void CheckIfShapeCanBePlaced()
    {
        var squareIndices = new List<Vector3Int>();
        foreach (var square in grid)
        {
            if (square.GetComponent<GridTile>().isHoover)
            {
                squareIndices.Add(square.GetComponent<GridTile>().TileIndex);
            }
        }
        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return; //there's no selected shape

        if (currentSelectedShape.TotalTriangleNumber == squareIndices.Count)
        {
            Debug.Log($"place? {currentSelectedShape.TotalTriangleNumber == squareIndices.Count}");
            foreach (Vector3Int i in squareIndices)
            {
                grid[i[0], i[1], i[2]].GetComponent<GridTile>().PlaceShapeOnBoard();

                //TODO: add squareIndices vao dict solution
            }
            CheckIfGameOver();

            int shapeLeft = 0;
            foreach (Shape shape in shapeStorage.shapeList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfSquareActive())
                    shapeLeft++;
            }

            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShapes();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }
            // CheckIfCompleted();

        }
        else
        {
            Debug.Log("MoveShapeToStartPosition");
            Debug.Log($"currentSelectedShape.TotalTriangleNumber {currentSelectedShape.TotalTriangleNumber}");
            Debug.Log($"squareIndices.Count {squareIndices.Count}");
            GameEvents.MoveShapeToStartPosition();
        }
    }

    private void SpawnLevel()
    {
        foreach (Vector3Int v in GameData.tileIndices)
        {
            grid[v.x, v.y, v.z].isInSample = true;
            grid[v.x, v.y, v.z].SetThisTileAsSample();
        }
        ClearGridAndSpawnShapes();
    }

    void CheckIfGameOver()
    {
        List<Vector3Int> visibleTiles = GetVisibleTiles();
        Debug.Log("Check if game over" + visibleTiles.Count.ToString() + " " + GameData.tileIndices.Count.ToString());
        if (AreListsEqualIgnoreOrder(visibleTiles, GameData.tileIndices))
        {
            GameEvents.GameOver();
        }

    }

    public List<Vector3Int> GetVisibleTiles()
    {
        List<Vector3Int> sampleTiles = new List<Vector3Int>();
        foreach (var tile in grid)
        {
            var gridTile = tile.GetComponent<GridTile>();
            if (gridTile.isVisible)
            {
                sampleTiles.Add(gridTile.TileIndex);
            }
        }
        return sampleTiles;
    }
    bool AreListsEqualIgnoreOrder(List<Vector3Int> list1, List<Vector3Int> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        var grouped1 = list1.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
        var grouped2 = list2.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());

        return grouped1.Count == grouped2.Count &&
            grouped1.All(pair => grouped2.TryGetValue(pair.Key, out int count) && count == pair.Value);
    }
}
