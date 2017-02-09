using UnityEngine;
using System.Collections;

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



}
