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
    public Tile tileOccuping;

    private Class classType;
    public int speed;

    public Class ClassType {
        set
        {
            classType = value;
            switch (classType)
            {
                case Class.Villager:
                    speed = 3;
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

    public void MoveTo(GameObject target)
    {
        StartCoroutine(AnimateMovement(target.transform.position));
        x = target.GetComponent<Tile>().x;
        y = target.GetComponent<Tile>().y;
        tileOccuping = target.GetComponent<Tile>();
    }

    IEnumerator AnimateMovement(Vector3 target)
    {
        Vector3 startPos = transform.position;

        float elapsedTime = 0;
        float timeNeeded = (Vector3.Distance(startPos, target) / moveAnimationSpeed);

        if(elapsedTime != timeNeeded)
        {
            while (elapsedTime <= timeNeeded)
            {
                transform.position = Vector3.Lerp(startPos, target, (elapsedTime / timeNeeded));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = target;
        }
    }

}
