using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Classes of human.
/// </summary>
public enum Class
{
    Villager,
    Knight,
    Farmer,
    Swordsman,
    Archer
}

/// <summary>
/// Author: Andrew Seba
/// Description: Human entity with types of classes.
/// </summary>
public class Human : Entity {

    public float moveAnimationSpeed = 2f;

    private Class classType;
    private int speed;

    public Class ClassType {
        set
        {
            classType = value;
            switch (classType)
            {
                case Class.Villager:
                    speed = 5;
                    break;
                case Class.Knight:
                    speed = 3;
                    break;
                case Class.Swordsman:
                    speed = 4;
                    break;
            }
        }
        get
        {
            return classType;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public void SetupHuman(Class pType, Faction pFaction, int pX, int pY)
    {
        ClassType = pType;
        Faction = pFaction;
        x = pX;
        y = pY;
    }

    public void MoveTo(GameObject target)
    {
        tileOccuping.occupiedBy = null;
        StartCoroutine(AnimateMovement(target));
        x = target.GetComponent<Tile>().x;
        y = target.GetComponent<Tile>().y;
        tileOccuping = target.GetComponent<Tile>();
        target.GetComponent<Tile>().occupiedBy = this;
    }

    IEnumerator AnimateMovement(GameObject target)
    {
        ScriptDijkstra dijkstra = new ScriptDijkstra();

        List<GameObject> path = dijkstra.PathFindDijkstra(tileOccuping.gameObject, target, this);


        Debug.Log("Starting while loop");
        Debug.Log("Does path contain target? Answer: " + path.Contains(target));
        if(path != null)
        {
            foreach (GameObject node in path)
            {
                Debug.Log("Node: " + node.name);
                Vector3 startPos = transform.position;

                float elapsedTime = 0;
                float timeNeeded = (Vector3.Distance(startPos, node.transform.position) / moveAnimationSpeed);
                Debug.Log("Time needed: " + timeNeeded);
                if (timeNeeded != 0 && elapsedTime != timeNeeded)
                {
                    while (elapsedTime <= timeNeeded)
                    {
                        transform.position = Vector3.Lerp(startPos, node.transform.position, (elapsedTime / timeNeeded));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = node.transform.position;
                }
            }

        }
    }

}
