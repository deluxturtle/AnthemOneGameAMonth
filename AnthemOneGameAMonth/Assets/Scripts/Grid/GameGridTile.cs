using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Andrew Seba
/// Description: Individual Tile Elements
/// </summary>
public class GameGridTile : MonoBehaviour {
    
    //private int gridX;
    //private int gridY;
    [SerializeField]
    private GameGridTile north;
    [SerializeField]
    private GameGridTile east;
    [SerializeField]
    private GameGridTile south;
    [SerializeField]
    private GameGridTile west;

    //public int GridX
    //{
    //    get
    //    {
    //        return gridX;
    //    }
    //    set
    //    {
    //        gridX = value;
    //    }
    //}

    //public int GridY
    //{
    //    get
    //    {
    //        return gridY;
    //    }
    //    set
    //    {
    //        gridY = value;
    //    }
    //}

    public GameGridTile North
    {
        get
        {
            return north;
        }
        set
        {
            north = value;
        }
    }

    public GameGridTile East
    {
        get
        {
            return east;
        }
        set
        {
            east = value;
        }
    }

    public GameGridTile South
    {
        get
        {
            return south;
        }
        set
        {
            south = value;
        }
    }

    public GameGridTile West
    {
        get
        {
            return west;
        }
        set
        {
            west = value;
        }
    }


}
