using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

    public int x, y;
    public bool isOccupied;
    public List<ScriptConnection> Connections = new List<ScriptConnection>();
}
