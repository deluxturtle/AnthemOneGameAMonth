using UnityEngine;
using System.Collections;

public class ScriptConnection {

    public GameObject from;
    public GameObject goingTo;
    public int cost;

    public ScriptConnection(GameObject pFrom, GameObject pGoing, int pCost)
    {
        from = pFrom;
        goingTo = pGoing;
        cost = pCost;
    }
}
