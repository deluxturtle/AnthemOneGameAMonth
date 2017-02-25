using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Combat classes of humans.
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

    [HideInInspector]
    public float moveAnimationSpeed = 3f; //How fast the human is animated moving on the board.

    private Class classType;
    private int speed;
    private float defence;
    private int adjacentEnemy = 0;

    #region PublicAccessors
    public Class ClassType {
        set
        {
            classType = value;
            switch (classType)
            {
                case Class.Villager:
                    speed = 5;
                    defence = 10f;
                    break;
                case Class.Knight:
                    speed = 3;
                    defence = 20f;
                    break;
                case Class.Swordsman:
                    speed = 4;
                    defence = 15f;
                    break;
            }
        }
        get{ return classType;}
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

    public float Defence
    {
        get
        {
            if(adjacentEnemy > 1)
            {
                return defence / adjacentEnemy;
            }
            else
            {
                return defence;
            }
        }
        //set
        //{
        //    defence = value;
        //}
    }

    #endregion

    /// <summary>
    /// Initializes the humans type faction and x, y coordinates.
    /// </summary>
    /// <param name="pType">Combat class of the human.</param>
    /// <param name="pFaction">Team</param>
    /// <param name="pX">Starting Tile X</param>
    /// <param name="pY">Starting Tile Y</param>
    public void SetupHuman(Class pType, Faction pFaction, int pX, int pY)
    {
        ClassType = pType;
        Faction = pFaction;
        x = pX;
        y = pY;
    }


    #region Moving&Animation
    /// <summary>
    /// Moves the human to a nother tile animates the human moving and changes 
    /// the occupation of previous and next tiles.
    /// </summary>
    /// <param name="target">Tile Object containing x,y coordinates</param>
    public void MoveTo(GameObject target)
    {
        tileOccuping.occupiedBy = null;
        StartCoroutine(AnimateMovement(target));
        x = target.GetComponent<Tile>().x;
        y = target.GetComponent<Tile>().y;
        tileOccuping = target.GetComponent<Tile>();
        target.GetComponent<Tile>().occupiedBy = this;

        //Update surrounding tiles about combat status.
        UpdateCombatPosition();
    }

    /// <summary>
    /// Changes the defencive values of this unit and the units around him.
    /// </summary>
    void UpdateCombatPosition()
    {
        foreach(GameObject humanObj in GameObject.FindGameObjectsWithTag("Entity"))
        {
            Entity entity = humanObj.GetComponent<Entity>();
            if(entity is Human)
            {
                Human human = (Human)entity;
                human.adjacentEnemy = 0;
                foreach (ScriptConnection conn in human.tileOccuping.Connections)
                {
                    if (conn.goingTo.GetComponent<Tile>().occupiedBy != null &&
                        conn.goingTo.GetComponent<Tile>().occupiedBy.Faction != human.Faction)
                    {
                        human.adjacentEnemy++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Lerps the humans position using dijkstra to the target tile.
    /// </summary>
    /// <param name="target">The tile the human is moving towards.</param>
    /// <returns></returns>
    IEnumerator AnimateMovement(GameObject target)
    {
        ScriptDijkstra dijkstra = new ScriptDijkstra();

        List<GameObject> path = dijkstra.PathFindDijkstra(tileOccuping.gameObject, target, this);
        
        if(path != null)
        {
            foreach (GameObject node in path)
            {
                Vector3 startPos = transform.position;

                float elapsedTime = 0;
                float timeNeeded = (Vector3.Distance(startPos, node.transform.position) / moveAnimationSpeed);
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
    #endregion
}
