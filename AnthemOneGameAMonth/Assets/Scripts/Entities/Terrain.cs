using UnityEngine;
using System.Collections;

public enum TerrainType
{
    Plain,
    Hill,
    Forest,
    Mountain
}

public class Terrain : Entity {

    public TerrainType terrainType = TerrainType.Plain;
    public int terrainCost = 0;
}
