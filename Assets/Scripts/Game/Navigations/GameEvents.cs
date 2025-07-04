using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{ 
    public static Action GameOver;
    public static Action<int> AddScore;
    public static Action CheckIfShapeCanBePlaced;
    public static Action PlaceShapeOnBoard;

    public static Action MoveShapeToStartPosition;
    public static Action SetShapeInactive;
    public static Action RequestNewShapes;
    public static Action ClearGrid;
    public Dictionary<(int, int), List<int[]>> LevelDatabase = new Dictionary<(int, int), List<int[]>> { };
    
}
