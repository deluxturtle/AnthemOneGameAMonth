using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Classes of human.
/// </summary>
public enum Class
{
    Villager,
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
                    speed = 1;
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
    }

    IEnumerator AnimateMovement(Vector3 target)
    {
        Vector3 startPos = transform.position;

        float elapsedTime = 0;
        float timeNeeded = (Vector3.Distance(startPos, target) / moveAnimationSpeed);

        while(elapsedTime <= timeNeeded)
        {
            transform.position = Vector3.Lerp(startPos, target, (elapsedTime / timeNeeded));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }

}
