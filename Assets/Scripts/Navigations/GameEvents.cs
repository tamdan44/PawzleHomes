using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<int> GameOver;
    public static Action GridAppears;
    public static Action<int, int> OpenLevel;
    public static Action CheckIfShapeCanBePlaced;
    public static Action PlaceShapeOnBoard;

    public static Action RequestNewShapes;
    public static Action ClearGrid;
    
}
