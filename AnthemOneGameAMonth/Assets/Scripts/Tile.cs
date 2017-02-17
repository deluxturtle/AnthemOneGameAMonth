using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Author: Andrew Seba
/// Description: Base class for tiles holds grid x,y and some pathfinding values.
/// </summary>
public class Tile : MonoBehaviour {

    public int x, y;
    public int range;
    public bool isOccupied;
    public List<ScriptConnection> Connections = new List<ScriptConnection>();
}
