using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Author: Andrew Seba
/// Description: Base class for tiles holds grid x,y and some pathfinding values.
/// </summary>
public class Tile : MonoBehaviour {

    public int x, y;
    public int range;
    public Human occupiedBy;
    public List<ScriptConnection> Connections = new List<ScriptConnection>();

    public bool IsOccupied()
    {
        if(occupiedBy == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
