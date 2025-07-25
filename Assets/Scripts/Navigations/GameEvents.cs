using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action GridAppears;
    public static Action<int> LevelCleared;
    public static Action<int, int> OpenLevel;
    public static Action CheckIfShapeCanBePlaced;
    // public static Action TurnOnHoover;
    // public static Action TurnOffHoover;
    public static Action PlaceShapeOnBoard;
    public static Action RequestNewShapes;
    public static Action ClearGrid;

    public static Action<int> AddCoins;
    public static Action<int> AddBigCoins;


    
}
