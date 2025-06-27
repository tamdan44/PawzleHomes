using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[][] column;
        private int _size;

        public Row(int size, bool[] tris)
        {
            CreateRow(size, tris);
        }

        public void CreateRow(int size, bool[] tris)
        {
            _size = size;
            column = new bool[_size][];
            UpdateTriangles(tris);
        }

        public void ClearRow()
        {
            for (int t = 0; t < 4; t++)
            {
                for (int i = 0; i < _size; i++)
                {
                    column[i][t] = false;
                }
            }
        }
        public void UpdateTriangles(bool[] tris)
        {
            for (int i = 0; i < _size; i++)
            {
                column[i] = tris[(i * 4)..(i * 4 + 4)];
            }
        }

    }

    public int columns = 0;
    public int rows = 0;
    public bool[] triangles;
    public Row[] board;
    private bool isUpdatingFromEditor = false;

    private void OnValidate()
    {
        int newSize = rows * columns * 4;
        if (triangles == null || triangles.Length != newSize)
        {
            triangles = new bool[newSize];
        }
        if (!isUpdatingFromEditor)
        {
            CreateNewBoard();
        }
    }

    public void Clear()
    {
        for (var i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
        UpdateTrianglesFromBoard();
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];

        for (var i = 0; i < rows && i < board.Length; i++)
        {
            board[i] = new Row(columns, triangles[(i * columns * 4)..(i * columns * 4 + columns * 4)]);
        }
    }
    // Method to update triangles array from board data
    public void UpdateTrianglesFromBoard()
    {
        if (board == null || triangles == null) return;
        
        for (int i = 0; i < rows && i < board.Length; i++)
        {
            board[i].UpdateTriangles(triangles[(i * columns * 4)..(i * columns * 4 + columns * 4)]);
        }
    }

    // Method called from editor when triangles are modified directly
    public void UpdateBoardFromTriangles()
    {
        CreateNewBoard();
        isUpdatingFromEditor = false;
    }
}
