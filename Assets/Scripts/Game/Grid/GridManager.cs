using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GridManager : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public GridTile _tilePrefab;
    public GridTile[,,] grid;
    public Dictionary<int, List<Vector3Int>> shapeCurrentPositions;

    [SerializeField] private int _width, _height;
    [SerializeField] private float _gridTileScale, everySquareOffset, _offsetTilePos;
    [SerializeField] private CanvasScale canvas;

    private Vector2 _offset = Vector2.zero;

    void Start()
    {
        shapeCurrentPositions = new();
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

    void GiveHint()
    {
        List<int> shapeLeft = new List<int>();

        foreach (Shape shape in shapeStorage.shapeList)
        {
            if (shape.IsOnStartPosition())
                shapeLeft.Add(shape.shapeIndex);
        }

        if (shapeLeft.Count == 0)
        {
            //TODO
            // message "no more shape"
            Debug.Log("no more shape");
        }
        else
        {
            // one shape move to its right place
        }
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
            shapeStorage.shapeList[i].shapeDataIndex = GameData.shapeDataIndices[i];
            GameEvents.RequestNewShapes();
        }
    }
    void SpawnGridTiles()
    {
        Vector3[,,] GridTilePosition = new Vector3[_width, _height, 4];
        Vector2 startPosition = new(0f, 0f);
        float relativeTilePos = _gridTileScale * _offsetTilePos;
        for (int t = 0; t < 4; t++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 90 * t + 135);
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 position_offset = t switch
                    {
                        0 => new Vector3(0, -1), //bot
                        1 => new Vector3(1, 0), //right
                        2 => new Vector3(0, 1), //top
                        3 => new Vector3(-1, 0), //left
                        _ => Vector3.zero

                    };
                    grid[x, y, t] = Instantiate(_tilePrefab, transform);
                    grid[x, y, t].transform.localRotation = rotation;
                    grid[x, y, t].transform.localScale *= _gridTileScale;
                    grid[x, y, t].TileIndex = new Vector3Int(x, y, t);

                    var tri_rect = grid[x, y, t].GetComponent<RectTransform>();
                    _offset.x = tri_rect.rect.width * canvas.GetScaleSize() + _gridTileScale * everySquareOffset - 5 / canvas.GetScaleSize();
                    _offset.y = tri_rect.rect.width * canvas.GetScaleSize() + _gridTileScale * everySquareOffset - 5 / canvas.GetScaleSize();
                    Vector3 pos_offset = new Vector3(startPosition.x + _offset.x * x, startPosition.y - _offset.y * y) + position_offset * relativeTilePos;
                    grid[x, y, t].GetComponent<RectTransform>().anchoredPosition = pos_offset;
                    GridTilePosition[x, y, t] = grid[x, y, t].transform.position;
                }
            }
        }
        // GameData.GridTilePosition = GridTilePosition;
    }
    //check if shape can be placed, if it can, place it on the grid, check and add scores
    private void CheckIfShapeCanBePlaced()
    {
        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return; //there's no selected shape

        var squareIndices = new List<Vector3Int>();
        foreach (var square in grid)
        {
            GridTile gridTile = square.GetComponent<GridTile>();
            if (gridTile.isHoover)
            {
                squareIndices.Add(gridTile.TileIndex);
            }
        }

        if (currentSelectedShape.TotalTriangleNumber == squareIndices.Count)
        {
            Debug.Log($"place? {currentSelectedShape.TotalTriangleNumber == squareIndices.Count}");

            foreach (Vector3Int i in squareIndices)
            {
                shapeCurrentPositions[currentSelectedShape.shapeIndex] = squareIndices;
                grid[i[0], i[1], i[2]].SwitchShapeVisibility();
                grid[i[0], i[1], i[2]].collisionShapeIndices.Add(currentSelectedShape.shapeIndex);
                // currentSelectedShape.PlaceShapeOnBoard(squareIndices);
            }
            GameData.onBoardShapes[currentSelectedShape.shapeIndex] = true;
            currentSelectedShape.MakeShapeInvisible();
            CheckIfGameOver();

        }
        else
        {
            currentSelectedShape.MoveShapeToStartPosition();
            Debug.Log($"currentSelectedShape.MoveShapeToStartPosition();{currentSelectedShape.shapeIndex}");
        }
    }

    private void SpawnLevel()
    {
        if (GameData.tileIndices == null)
        {
            return;
        }
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
        List<Vector3Int> sampleTiles = new();
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
        if (list1.Count != list2.Count) return false;

        var grouped1 = list1.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
        var grouped2 = list2.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());

        return grouped1.Count == grouped2.Count &&
            grouped1.All(pair => grouped2.TryGetValue(pair.Key, out int count) && count == pair.Value);
    }
}
