using UnityEngine;
using System.Collections;

public enum Faction
{
    Red,
    Blue,
    Green,
    Yellow,
    White
}

/// <summary>
/// Author: Andrew Seba
/// Description: Base class for all entity tiles including humans and all of that sort.
/// </summary>
public class Entity : Tile {

    string nickname;
    private Faction faction = Faction.White; //white is neutral.
    public Tile tileOccuping;

    public Faction Faction
    {
        get
        {
            return faction;
        }
        set
        {
            faction = value;
        }
    }

}
